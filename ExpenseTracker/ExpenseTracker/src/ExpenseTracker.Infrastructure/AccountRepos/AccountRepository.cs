using ErrorOr;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Domain.Errors.AccountErrors;
using ExpenseTracker.Domain.Errors.DatabaseErrors;
using ExpenseTracker.Infrastructure.AccountRepos.Options;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Security.Principal;

namespace ExpenseTracker.Infrastructure.AccountRepos

{
    internal class AccountRepository : IAccountRepository
    {
        private readonly AccountOptions _options;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(AccountOptions options, ILogger<AccountRepository> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<ErrorOr<List<Account>>> GetAccountsAsync(CancellationToken token)
        {

            var accounts = new List<Account>();

            try
            {
                _logger.LogInformation("Fetching all accounts from database");

                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                    SELECT userID, username, email, passwordHash, realName, realSurname, phoneNumber,
                           createdAt, lastLoginAt, isActive, refreshToken, refreshTokenExpiryTime
                    FROM Account
                    ORDER BY userID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;

                using var reader = await command.ExecuteReaderAsync(token);

                while (await reader.ReadAsync(token))
                {
                    accounts.Add(new Account
                    {
                        userID = reader.GetInt32(0),
                        username = reader.GetString(1),
                        email = reader.GetString(2),
                        passwordHash = reader.GetString(3),
                        realName = reader.GetString(4),
                        realSurname = reader.GetString(5),
                        phoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                        createdAt = reader.GetDateTime(7),
                        lastLoginAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                        isActive = reader.GetBoolean(9),
                        refreshToken = reader.IsDBNull(10) ? null : reader.GetString(10),
                        refreshTokenExpiryTime = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
                    });
                }

                return accounts;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError(ex, "Database timeout fetching accounts");
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                _logger.LogError(ex, "Database connection failed fetching accounts");
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error fetching accounts");
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation cancelled fetching accounts");
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching accounts");
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Account>> GetAccountByIdAsync(int userId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                    SELECT userID, username, email, passwordHash, realName, realSurname, phoneNumber,
                           createdAt, lastLoginAt, isActive, refreshToken, refreshTokenExpiryTime
                    FROM Account
                    WHERE userID = @UserId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@UserId", userId);

                using var reader = await command.ExecuteReaderAsync(token);

                if (await reader.ReadAsync(token))
                {
                    _logger.LogInformation("Account found: {UserId}", userId);
                    return new Account
                    {
                        userID = reader.GetInt32(0),
                        username = reader.GetString(1),
                        email = reader.GetString(2),
                        passwordHash = reader.GetString(3),
                        realName = reader.GetString(4),
                        realSurname = reader.GetString(5),
                        phoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                        createdAt = reader.GetDateTime(7),
                        lastLoginAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                        isActive = reader.GetBoolean(9),
                        refreshToken = reader.IsDBNull(10) ? null : reader.GetString(10),
                        refreshTokenExpiryTime = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
                    };
                }

                _logger.LogWarning("Account not found: {UserId}", userId);
                return AccountErrors.NotFound.Account;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError(ex, "Database timeout fetching account: {UserId}", userId);
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                _logger.LogError(ex, "Database connection failed fetching account: {UserId}", userId);
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error fetching account: {UserId}", userId);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching account: {UserId}", userId);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Account>> CreateAccountAsync(Account account, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Creating account for email: {Email}", account.email);
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                    INSERT INTO Account (username, email, passwordHash, realName, realSurname, phoneNumber, 
                                        createdAt, isActive)
                    VALUES (@Username, @Email, @PasswordHash, @RealName, @RealSurname, @PhoneNumber, 
                           @CreatedAt, @IsActive)
                    RETURNING userID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@Username", account.username);
                command.Parameters.AddWithValue("@Email", account.email);
                command.Parameters.AddWithValue("@PasswordHash", account.passwordHash);
                command.Parameters.AddWithValue("@RealName", account.realName);
                command.Parameters.AddWithValue("@RealSurname", account.realSurname);
                command.Parameters.AddWithValue("@PhoneNumber", (object?)account.phoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreatedAt", account.createdAt);
                command.Parameters.AddWithValue("@IsActive", account.isActive);

                var userId = await command.ExecuteScalarAsync(token);

                if (userId == null)
                {
                    _logger.LogError("Failed to create account - no ID returned");
                    return DatabaseErrors.Database.OperationFailed;
                }

                account.userID = Convert.ToInt32(userId);

                _logger.LogInformation("Account created successfully: {UserId}", account.userID);
                return account;
            }
            catch (NpgsqlException ex) when (ex.SqlState == "23505") 
            {
                _logger.LogWarning("Duplicate constraint violation creating account: {Email}", account.email);

                if (ex.Message.Contains("email"))
                    return AccountErrors.Database.DuplicateEmail;
                if (ex.Message.Contains("username"))
                    return AccountErrors.Database.DuplicateUsername;

                return DatabaseErrors.Database.OperationFailed;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error creating account: {Email}", account.email);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating account: {Email}", account.email);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Updated>> UpdateAccountAsync(Account account, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Updating account: {UserId}", account.userID);
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                    UPDATE Account
                    SET username = @Username,
                        email = @Email,
                        passwordHash = @PasswordHash,
                        realName = @RealName,
                        realSurname = @RealSurname,
                        phoneNumber = @PhoneNumber,
                        lastLoginAt = @LastLoginAt,
                        isActive = @IsActive,
                        refreshToken = @RefreshToken,
                        refreshTokenExpiryTime = @RefreshTokenExpiryTime
                    WHERE userID = @UserId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;

                command.Parameters.AddWithValue("@UserId", account.userID);
                command.Parameters.AddWithValue("@Username", account.username);
                command.Parameters.AddWithValue("@Email", account.email);
                command.Parameters.AddWithValue("@PasswordHash", account.passwordHash);
                command.Parameters.AddWithValue("@RealName", account.realName);
                command.Parameters.AddWithValue("@RealSurname", account.realSurname);
                command.Parameters.AddWithValue("@PhoneNumber", (object?)account.phoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastLoginAt", (object?)account.lastLoginAt ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", account.isActive);
                command.Parameters.AddWithValue("@RefreshToken", (object?)account.refreshToken ?? DBNull.Value);
                command.Parameters.AddWithValue("@RefreshTokenExpiryTime", (object?)account.refreshTokenExpiryTime ?? DBNull.Value);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("Account not found for update: {UserId}", account.userID);
                    return AccountErrors.NotFound.Account;
                }

                _logger.LogInformation("Account updated successfully: {UserId}", account.userID);
                return Result.Updated;
            }
            catch (NpgsqlException ex) when (ex.SqlState == "23505") 
            {
                _logger.LogWarning("Duplicate constraint violation updating account: {UserId}", account.userID);

                if (ex.Message.Contains("email"))
                    return AccountErrors.Database.DuplicateEmail;
                if (ex.Message.Contains("username"))
                    return AccountErrors.Database.DuplicateUsername;

                return DatabaseErrors.Database.OperationFailed;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error updating account: {UserId}", account.userID);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating account: {UserId}", account.userID);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Deleted>> DeleteAccountAsync(int userId, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Deleting account: {UserId}", userId);

                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "DELETE FROM Account WHERE userID = @UserId";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.CommandTimeout = 30;

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("Account not found for deletion: {UserId}", userId);
                    return AccountErrors.NotFound.Account;
                }

                _logger.LogInformation("Account deleted successfully: {UserId}", userId);
                return Result.Deleted;
            }
            catch (NpgsqlException ex) when (ex.SqlState == "23503") // Foreign key violation
            {
                _logger.LogWarning("Cannot delete account due to foreign key constraint: {UserId}", userId);
                return AccountErrors.Conflict.AccountInUse;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error deleting account: {UserId}", userId);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting account: {UserId}", userId);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Account>> GetByEmailAsync(string email, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Fetching account by email: {Email}", email);

                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                    SELECT userID, username, email, passwordHash, realName, realSurname, phoneNumber,
                           createdAt, lastLoginAt, isActive, refreshToken, refreshTokenExpiryTime
                    FROM Account
                    WHERE email = @Email";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.CommandTimeout = 30;

                using var reader = await command.ExecuteReaderAsync(token);

                if (await reader.ReadAsync(token))
                {
                    _logger.LogInformation("Account found: {UserId}", reader.GetInt32(0));
                    return new Account
                    {
                        userID = reader.GetInt32(0),
                        username = reader.GetString(1),
                        email = reader.GetString(2),
                        passwordHash = reader.GetString(3),
                        realName = reader.GetString(4),
                        realSurname = reader.GetString(5),
                        phoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                        createdAt = reader.GetDateTime(7),
                        lastLoginAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                        isActive = reader.GetBoolean(9),
                        refreshToken = reader.IsDBNull(10) ? null : reader.GetString(10),
                        refreshTokenExpiryTime = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
                    };
                }

                _logger.LogWarning("Account not found with email: {Email}", email);
                return AccountErrors.NotFound.Account;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error fetching account by email: {Email}", email);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching account by email: {Email}", email);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<bool>> EmailExistsAsync(string email, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Checking if email exists: {Email}", email);

                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "SELECT COUNT(*) FROM Account WHERE email = @Email";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.CommandTimeout = 30;

                var count = (long)(await command.ExecuteScalarAsync(token) ?? 0L);
                var exists = count > 0;

                _logger.LogInformation("Email exists check for {Email}: {Exists}", email, exists);
                return exists;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error checking email existence: {Email}", email);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error checking email existence: {Email}", email);
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<bool>> UsernameExistsAsync(string username, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Checking if username exists: {Username}", username);

                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "SELECT COUNT(*) FROM Account WHERE username = @Username";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.CommandTimeout = 30;

                var count = (long)(await command.ExecuteScalarAsync(token) ?? 0L);
                var exists = count > 0;

                _logger.LogInformation("Username exists check for {Username}: {Exists}", username, exists);
                return exists;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error checking username existence: {Username}", username);
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error checking username existence: {Username}", username);
                return DatabaseErrors.Database.OperationFailed;
            }
        }
    }
}
