using ErrorOr;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.Errors.DatabaseErrors;
using ExpenseTracker.Domain.Errors.StandardExpenseErrors;
using ExpenseTracker.Domain.Errors.TransactionErrors;
using ExpenseTracker.Domain.StandardExpenseData;
using ExpenseTracker.Infrastructure.StandardExpenseRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.StandardExpenseRepos
{
    internal class StandardExpenseRepository : IStandardExpenseRepository
    {
        private readonly StandardExpenseOptions _options;
        const string ForeignKeyViolation = "20503";
        const string UniqueViolation = "20505";

        public StandardExpenseRepository(StandardExpenseOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<ErrorOr<List<StandardExpense>>> GetStandardExpensesAsync(CancellationToken token)
        {

            var standardExpenses = new List<StandardExpense>();

            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT expenseID, walletID, reason, 
                       description, amount, frequency, nextDate
                FROM StandardExpense
                ORDER BY expenseID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                using var reader = await command.ExecuteReaderAsync(token);

                while (await reader.ReadAsync(token))
                {
                    standardExpenses.Add(new StandardExpense
                    {
                        expenseID = reader.GetInt32(0),
                        walletID = reader.GetInt32(1),
                        reason = reader.GetString(2),
                        description = reader.GetString(3),
                        amount = reader.GetDecimal(4),
                        frequency = reader.GetString(5),
                        nextDate = DateOnly.FromDateTime(reader.GetDateTime(6))

                    });
                }

                return standardExpenses;
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

        public async Task<ErrorOr<StandardExpense>> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT expenseID, walletID, reason, 
                       description, amount, frequency, nextDate
                FROM StandardExpense
                WHERE expenseID = @ExpenseId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@ExpenseId", expenseId);

                using var reader = await command.ExecuteReaderAsync(token);

                if (await reader.ReadAsync(token))
                {
                    return new StandardExpense
                    {
                        expenseID = reader.GetInt32(0),
                        walletID = reader.GetInt32(1),
                        reason = reader.GetString(2),
                        description = reader.GetString(3),
                        amount = reader.GetDecimal(4),
                        frequency = reader.GetString(5),
                        nextDate = DateOnly.FromDateTime(reader.GetDateTime(6))
                    };
                }

                return StandardExpenseErrors.NotFound.StandardExpense;
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

        public async Task<ErrorOr<StandardExpense>> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                INSERT INTO StandardExpense (walletID, reason, description, amount, frequency, nextDate)
                VALUES (@WalletID, @Reason, @Description, @Amount, @Frequency, @NextDate)
                RETURNING expenseID";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@WalletID", standardExpense.walletID);
                command.Parameters.AddWithValue("@Reason", standardExpense.reason);
                command.Parameters.AddWithValue("@Description", standardExpense.description);
                command.Parameters.AddWithValue("@Amount", standardExpense.amount);
                command.Parameters.AddWithValue("@Frequency", standardExpense.frequency);
                command.Parameters.AddWithValue("@NextDate", standardExpense.nextDate);

                var expenseId = await command.ExecuteScalarAsync(token);
                if (expenseId == null)
                {
                    return DatabaseErrors.Database.OperationFailed;
                }
                standardExpense.expenseID = Convert.ToInt32(expenseId);

                return standardExpense;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateRow;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                if (ex.Message.Contains("Wallet"))
                    return StandardExpenseErrors.Validation.InvalidWalletId;

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

        public async Task<ErrorOr<Updated>> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                UPDATE StandardExpense
                SET walletID = @WalletID,
                    reason = @Reason,
                    description = @Description,
                    amount = @Amount,
                    frequency = @Frequency,
                    nextDate = @NextDate
                WHERE expenseID = @ExpenseId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@ExpenseId", standardExpense.expenseID);
                command.Parameters.AddWithValue("@WalletID", standardExpense.walletID);
                command.Parameters.AddWithValue("@Reason", standardExpense.reason);
                command.Parameters.AddWithValue("@Description", standardExpense.description);
                command.Parameters.AddWithValue("@Amount", standardExpense.amount);
                command.Parameters.AddWithValue("@Frequency", standardExpense.frequency);
                command.Parameters.AddWithValue("@NextDate", standardExpense.nextDate);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);
                if(rowsAffected == 0)
                {
                    return StandardExpenseErrors.NotFound.StandardExpense;
                }

                return Result.Updated;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateRow;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                if (ex.Message.Contains("Wallet"))
                    return StandardExpenseErrors.Validation.InvalidWalletId;

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

        public async Task<ErrorOr<Deleted>> DeleteStandardExpenseAsync(int expenseId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "DELETE FROM StandardExpense WHERE expenseID = @ExpenseId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@ExpenseId", expenseId);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if(rowsAffected == 0)
                {
                    return StandardExpenseErrors.NotFound.StandardExpense;
                }

                return Result.Deleted;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return StandardExpenseErrors.Conflict.StandardExpenseInUse;
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
