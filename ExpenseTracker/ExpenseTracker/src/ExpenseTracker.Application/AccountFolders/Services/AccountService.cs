using ExpenseTracker.Application.AccountFolders.Data;
using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.AccountData;
using Microsoft.Extensions.Logging;
using ErrorOr;

namespace ExpenseTracker.Application.AccountFolders.Services

{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        public async Task<ErrorOr<AllAccounts>> GetAccountsAsync(CancellationToken token)
        {
            _logger.LogInformation("Fetching all accounts");

            var getAccounts = await _accountRepository.GetAccountsAsync(token);

            if (getAccounts.IsError)
            {
                _logger.LogError("Failed to fetch accounts");
                return getAccounts.Errors;
            }

            _logger.LogInformation("Successfully fetched {Count} accounts", getAccounts.Value.Count);

            return new AllAccounts
            {
                accounts = getAccounts.Value
            };
        }

        public async Task<ErrorOr<Account>> GetAccountByIdAsync(int userId, CancellationToken token)
        {
            _logger.LogInformation("Fetching account by ID: {UserId}", userId);

            // Validate input
            var validationResult = AccountValidator.ValidateUserId(userId);
            if (validationResult.IsError)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return validationResult.Errors;
            }

            var result = await _accountRepository.GetAccountByIdAsync(userId, token);

            if (result.IsError)
            {
                _logger.LogWarning("Account not found: {UserId}", userId);
                return result.Errors;
            }

            _logger.LogInformation("Successfully fetched account: {UserId}", userId);
            return result.Value;
        }

        public async Task<ErrorOr<Account>> CreateAccountAsync(Account account, CancellationToken token)
        {
            _logger.LogInformation("Creating account for email: {Email}", account.email);

            // Validate account
            var validationResult = AccountValidator.ValidateForCreate(account);
            if (validationResult.IsError)
            {
                _logger.LogWarning("Account validation failed for email: {Email}", account.email);
                return validationResult.Errors;
            }

            var result = await _accountRepository.CreateAccountAsync(account, token);

            if (result.IsError)
            {
                _logger.LogError("Failed to create account for email: {Email}", account.email);
                return result.Errors;
            }

            _logger.LogInformation("Successfully created account: {UserId}", result.Value.userID);
            return result.Value;
        }

        public async Task<ErrorOr<Updated>> UpdateAccountAsync(Account account, CancellationToken token)
        {
            _logger.LogInformation("Updating account: {UserId}", account.userID);

            // Validate account
            var validationResult = AccountValidator.ValidateForUpdate(account);
            if (validationResult.IsError)
            {
                _logger.LogWarning("Account validation failed for update: {UserId}", account.userID);
                return validationResult.Errors;
            }

            var result = await _accountRepository.UpdateAccountAsync(account, token);

            if (result.IsError)
            {
                _logger.LogError("Failed to update account: {UserId}", account.userID);
                return result.Errors;
            }

            _logger.LogInformation("Successfully updated account: {UserId}", account.userID);
            return result.Value;
        }

        public async Task<ErrorOr<Deleted>> DeleteAccountAsync(int userId, CancellationToken token)
        {
            _logger.LogInformation("Deleting account: {UserId}", userId);

            // Validate input
            var validationResult = AccountValidator.ValidateUserId(userId);
            if (validationResult.IsError)
            {
                _logger.LogWarning("Invalid user ID for deletion: {UserId}", userId);
                return validationResult.Errors;
            }

            var result = await _accountRepository.DeleteAccountAsync(userId, token);

            if (result.IsError)
            {
                _logger.LogError("Failed to delete account: {UserId}", userId);
                return result.Errors;
            }

            _logger.LogInformation("Successfully deleted account: {UserId}", userId);
            return result.Value;
        }
    }
}
