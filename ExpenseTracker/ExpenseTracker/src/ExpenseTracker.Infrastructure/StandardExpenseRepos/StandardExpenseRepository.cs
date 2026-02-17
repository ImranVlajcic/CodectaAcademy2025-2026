using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.StandardExpenseData;
using ExpenseTracker.Infrastructure.StandardExpenseRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.StandardExpenseRepos
{
    internal class StandardExpenseRepository : IStandardExpenseRepository
    {
        private readonly StandardExpenseOptions _options;

        public StandardExpenseRepository(StandardExpenseOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<StandardExpense>> GetStandardExpensesAsync(CancellationToken token)
        {

            var standardExpenses = new List<StandardExpense>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT expenseID, walletID, reason, 
                       description, amount, frequency, nextDate
                FROM StandardExpense
                ORDER BY expenseID";

            using var command = new NpgsqlCommand(sql, connection);
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

        public async Task<StandardExpense?> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT expenseID, walletID, reason, 
                       description, amount, frequency, nextDate
                FROM StandardExpense
                WHERE expenseID = @ExpenseId";

            using var command = new NpgsqlCommand(sql, connection);
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

            return null;
        }

        public async Task<StandardExpense> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
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
            standardExpense.expenseID = Convert.ToInt32(expenseId);

            return standardExpense;
        }

        public async Task<bool> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
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
            command.Parameters.AddWithValue("@ExpenseId", standardExpense.expenseID);
            command.Parameters.AddWithValue("@WalletID", standardExpense.walletID);
            command.Parameters.AddWithValue("@Reason", standardExpense.reason);
            command.Parameters.AddWithValue("@Description", standardExpense.description);
            command.Parameters.AddWithValue("@Amount", standardExpense.amount);
            command.Parameters.AddWithValue("@Frequency", standardExpense.frequency);
            command.Parameters.AddWithValue("@NextDate", standardExpense.nextDate);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStandardExpenseAsync(int expenseId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM StandardExpense WHERE expenseID = @ExpenseId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ExpenseId", expenseId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
