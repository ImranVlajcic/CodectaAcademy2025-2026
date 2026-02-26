using ErrorOr;
using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Domain.Errors.AccountErrors;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;


namespace ExpenseTracker.Application.AccountFolders.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IAccountRepository accountRepository,
            IJwtTokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher,
            JwtOptions jwtOptions,
            ILogger<AuthenticationService> logger)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(
            string username,
            string email,
            string password,
            string realName,
            string realSurname,
            string phoneNumber,
            CancellationToken token)
        {
            _logger.LogInformation("Registration attempt for email: {Email}, username: {Username}", email, username);

            var validationResult = ValidateRegistrationInput(username, email, password, realName, realSurname, phoneNumber);
            if (validationResult.IsError)
            {
                _logger.LogWarning("Registration validation failed for {Email}", email);
                return validationResult.Errors;
            }

            var emailExistsResult = await _accountRepository.EmailExistsAsync(email, token);
            if (emailExistsResult.IsError)
            {
                return emailExistsResult.Errors;
            }

            if (emailExistsResult.Value)
            {
                _logger.LogWarning("Registration failed - email already exists: {Email}", email);
                return AccountErrors.Database.DuplicateEmail;
            }

            var usernameExistsResult = await _accountRepository.UsernameExistsAsync(username, token);
            if (usernameExistsResult.IsError)
            {
                return usernameExistsResult.Errors;
            }

            if (usernameExistsResult.Value)
            {
                _logger.LogWarning("Registration failed - username already exists: {Username}", username);
                return AccountErrors.Database.DuplicateUsername;
            }

            var passwordHash = _passwordHasher.HashPassword(password);

            var account = new Account
            {
                username = username,
                email = email.ToLowerInvariant(),
                passwordHash = passwordHash,
                realName = realName,
                realSurname = realSurname,
                phoneNumber = phoneNumber,
                createdAt = DateTime.UtcNow,
                isActive = true
            };

            var createResult = await _accountRepository.CreateAccountAsync(account, token);
            if (createResult.IsError)
            {
                _logger.LogError("Failed to create account: {Email}", email);
                return createResult.Errors;
            }

            var createdAccount = createResult.Value;

            var accessToken = _tokenGenerator.GenerateAccessToken(createdAccount);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            createdAccount.refreshToken = refreshToken;
            createdAccount.refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays);

            var updateResult = await _accountRepository.UpdateAccountAsync(createdAccount, token);
            if (updateResult.IsError)
            {
                _logger.LogError("Failed to save refresh token for account: {UserId}", createdAccount.userID);
                return updateResult.Errors;
            }

            _logger.LogInformation("Account registered successfully: {UserId}", createdAccount.userID);

            return new AuthenticationResult(
                createdAccount.userID,
                createdAccount.username,
                createdAccount.email,
                createdAccount.realName,
                createdAccount.realSurname,
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                createdAccount.refreshTokenExpiryTime.Value);
        }

        public async Task<ErrorOr<AuthenticationResult>> LoginAsync(
            string email,
            string password,
            CancellationToken token)
        {
            _logger.LogInformation("Login attempt for email: {Email}", email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return AccountErrors.Validation.EmailRequired;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return AccountErrors.Validation.PasswordRequired;
            }

            var accountResult = await _accountRepository.GetByEmailAsync(email.ToLowerInvariant(), token);
            if (accountResult.IsError)
            {
                _logger.LogWarning("Login failed - account not found: {Email}", email);
                return AccountErrors.Validation.InvalidCredentials;
            }

            var account = accountResult.Value;

            if (!account.isActive)
            {
                _logger.LogWarning("Login failed - account inactive: {UserId}", account.userID);
                return AccountErrors.Validation.AccountInactive;
            }

            if (!_passwordHasher.VerifyPassword(password, account.passwordHash))
            {
                _logger.LogWarning("Login failed - invalid password for account: {UserId}", account.userID);
                return AccountErrors.Validation.InvalidCredentials;
            }

            var accessToken = _tokenGenerator.GenerateAccessToken(account);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            account.lastLoginAt = DateTime.UtcNow;
            account.refreshToken = refreshToken;
            account.refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays);

            var updateResult = await _accountRepository.UpdateAccountAsync(account, token);
            if (updateResult.IsError)
            {
                _logger.LogError("Failed to update account login info: {UserId}", account.userID);
            }

            _logger.LogInformation("Account logged in successfully: {UserId}", account.userID);

            return new AuthenticationResult(
                account.userID,
                account.username,
                account.email,
                account.realName,
                account.realSurname,
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                account.refreshTokenExpiryTime.Value);
        }

        public async Task<ErrorOr<AuthenticationResult>> RefreshTokenAsync(
            string refreshToken,
            CancellationToken token)
        {
            _logger.LogInformation("Refresh token attempt");

            
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogWarning("Refresh token is empty");
                return AccountErrors.Validation.InvalidRefreshToken;
            }

            
            var accountResult = await _accountRepository.GetByRefreshTokenAsync(refreshToken, token);
            if (accountResult.IsError)
            {
                _logger.LogWarning("Refresh token validation failed - no account found or token expired");
                return AccountErrors.Validation.InvalidRefreshToken;
            }

            var account = accountResult.Value;

           
            if (account.refreshTokenExpiryTime == null || account.refreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token expired for account: {UserId}", account.userID);
                return AccountErrors.Validation.InvalidRefreshToken;
            }

            
            if (!account.isActive)
            {
                _logger.LogWarning("Refresh token used on inactive account: {UserId}", account.userID);
                return AccountErrors.Validation.AccountInactive;
            }

            _logger.LogInformation("Refresh token validated for account: {UserId}", account.userID);

            
            var newAccessToken = _tokenGenerator.GenerateAccessToken(account);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            
            account.refreshToken = newRefreshToken;
            account.refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays);
            account.lastLoginAt = DateTime.UtcNow;  

            var updateResult = await _accountRepository.UpdateAccountAsync(account, token);
            if (updateResult.IsError)
            {
                _logger.LogError("Failed to update refresh token for account: {UserId}", account.userID);
                return updateResult.Errors;
            }

            _logger.LogInformation("New tokens generated successfully for account: {UserId}", account.userID);

            return new AuthenticationResult(
                account.userID,
                account.username,
                account.email,
                account.realName,
                account.realSurname,
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                account.refreshTokenExpiryTime.Value);
        }

        public async Task<ErrorOr<Success>> RevokeTokenAsync(int userId, CancellationToken token)
        {
            _logger.LogInformation("Revoking refresh token for account: {UserId}", userId);

            var accountResult = await _accountRepository.GetAccountByIdAsync(userId, token);
            if (accountResult.IsError)
            {
                return accountResult.Errors;
            }

            var account = accountResult.Value;
            account.refreshToken = null;
            account.refreshTokenExpiryTime = null;

            var updateResult = await _accountRepository.UpdateAccountAsync(account, token);
            if (updateResult.IsError)
            {
                _logger.LogError("Failed to revoke token for account: {UserId}", userId);
                return updateResult.Errors;
            }

            _logger.LogInformation("Refresh token revoked successfully for account: {UserId}", userId);
            return Result.Success;
        }

        private static ErrorOr<Success> ValidateRegistrationInput(
            string username,
            string email,
            string password,
            string realName,
            string realSurname,
            string phoneNumber)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add(AccountErrors.Validation.UsernameRequired);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(AccountErrors.Validation.EmailRequired);
            }
            else if (!IsValidEmail(email))
            {
                errors.Add(AccountErrors.Validation.InvalidEmail);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(AccountErrors.Validation.PasswordRequired);
            }
            else if (!IsStrongPassword(password))
            {
                errors.Add(AccountErrors.Validation.WeakPassword);
            }

            if (string.IsNullOrWhiteSpace(realName))
            {
                errors.Add(AccountErrors.Validation.RealNameRequired);
            }

            if (string.IsNullOrWhiteSpace(realSurname))
            {
                errors.Add(AccountErrors.Validation.RealSurnameRequired);
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber) && !IsValidPhoneNumber(phoneNumber))
            {
                errors.Add(AccountErrors.Validation.InvalidPhoneNumber);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }

        private static bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
                return false;

            var hasUpperCase = password.Any(char.IsUpper);
            var hasLowerCase = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^[\d\s\-\+\(\)]+$");
            return phoneRegex.IsMatch(phoneNumber) && phoneNumber.Length >= 10;
        }
    }
}
