using ErrorOr;
using ExpenseTracker.Application.WalletFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.Errors.DatabaseErrors;
using ExpenseTracker.Domain.Errors.TransactionErrors;
using ExpenseTracker.Domain.Errors.WalletErrors;
using ExpenseTracker.Domain.WalletData;
using ExpenseTracker.Infrastructure.WalletRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.WalletRepos
{
    internal class WalletRepository : IWalletRepository
    {
        private readonly WalletOptions _options;
        const string ForeignKeyViolation = "20503";
        const string UniqueViolation = "20505";

        public WalletRepository(WalletOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<ErrorOr<List<Wallet>>> GetWalletsAsync(CancellationToken token)
        {

            var wallets = new List<Wallet>();

            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT walletID, userID, currencyID, balance, purpose
                FROM Wallet
                ORDER BY walletID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                using var reader = await command.ExecuteReaderAsync(token);

                while (await reader.ReadAsync(token))
                {
                    wallets.Add(new Wallet
                    {
                        walletID = reader.GetInt32(0),
                        userID = reader.GetInt32(1),
                        currencyID = reader.GetInt32(2),
                        balance = reader.GetDecimal(3),
                        purpose = reader.GetString(4)
                    });
                }

                return wallets;
            }
            catch (NpgsqlException ex) when (ex.InnerException is Timeout)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Wallet>> GetWalletByIdAsync(int walletId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT walletID, userID, currencyID, balance, purpose
                FROM Wallet
                WHERE walletID = @WalletId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@WalletId", walletId);

                using var reader = await command.ExecuteReaderAsync(token);

                if (await reader.ReadAsync(token))
                {
                    return new Wallet
                    {
                        walletID = reader.GetInt32(0),
                        userID = reader.GetInt32(1),
                        currencyID = reader.GetInt32(2),
                        balance = reader.GetDecimal(3),
                        purpose = reader.GetString(4)
                    };
                }

                return WalletErrors.NotFound.Wallet;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Wallet>> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                INSERT INTO Wallet (userID, currencyID, balance, purpose)
                VALUES (@UserID, @CurrencyID, @Balance, @Purpose )
                RETURNING userID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@UserID", wallet.userID);
                command.Parameters.AddWithValue("@CurrencyID", wallet.currencyID);
                command.Parameters.AddWithValue("@Balance", wallet.balance);
                command.Parameters.AddWithValue("@Purpose", wallet.purpose);

                var walletId = await command.ExecuteScalarAsync(token);

                if (walletId == null)
                {
                    return DatabaseErrors.Database.OperationFailed;
                }

                wallet.walletID = Convert.ToInt32(walletId);

                return wallet;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                if (ex.Message.Contains("User"))
                    return WalletErrors.Validation.InvalidUserId;
                if (ex.Message.Contains("Currency"))
                    return WalletErrors.Validation.InvalidCurrencyId;;

                return DatabaseErrors.Database.OperationFailed;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Updated>> UpdateWalletAsync(Wallet wallet, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                UPDATE Wallet
                SET userID = @UserID,
                    currencyID = @CurrencyID,
                    balance = @Balance,
                    purpose = @Purpose
                WHERE walletID = @WalletId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@WalletID", wallet.walletID);
                command.Parameters.AddWithValue("@UserID", wallet.userID);
                command.Parameters.AddWithValue("@CurrencyID", wallet.currencyID);
                command.Parameters.AddWithValue("@Balance", wallet.balance);
                command.Parameters.AddWithValue("@Purpose", wallet.purpose);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if (rowsAffected == 0)
                {
                    return WalletErrors.NotFound.Wallet;
                }

                return Result.Updated;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                if (ex.Message.Contains("User"))
                    return WalletErrors.Validation.InvalidUserId;
                if (ex.Message.Contains("Currency"))
                    return WalletErrors.Validation.InvalidCurrencyId; ;

                return DatabaseErrors.Database.OperationFailed;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Deleted>> DeleteWalletAsync(int walletId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "DELETE FROM Wallet WHERE walletID = @WalletId";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@WalletId", walletId);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if (rowsAffected == 0) {
                    return WalletErrors.NotFound.Wallet;
                }

                return Result.Deleted;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return TransactionErrors.Conflict.TransactionInUse;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }
    }
}
