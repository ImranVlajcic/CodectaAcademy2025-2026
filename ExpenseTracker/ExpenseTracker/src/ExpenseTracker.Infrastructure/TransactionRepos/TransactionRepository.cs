using ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.TransactionData;
using ExpenseTracker.Infrastructure.TransactionRepos.Options;
using Npgsql;


namespace ExpenseTracker.Infrastructure.TransactionRepos
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionOptions _options;

        public TransactionRepository(TransactionOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<Transaction>> GetTransactionsAsync(CancellationToken token)
        {

            var transactions = new List<Transaction>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT transactionID, walletID, categoryID, currencyID, 
                       transactionTime, transactionDate, transactionType, amount, description
                FROM Transactions
                ORDER BY transactionID";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
            {
                transactions.Add(new Transaction
                {
                    transactionID = reader.GetInt32(0),
                    walletID = reader.GetInt32(1),
                    categoryID = reader.GetInt32(2),
                    currencyID = reader.GetInt32(3),
                    transactionTime = TimeOnly.FromDateTime(reader.GetDateTime(4)),
                    transactionDate = DateOnly.FromDateTime(reader.GetDateTime(5)),
                    transactionType = reader.GetString(6),
                    amount = reader.GetDecimal(7),
                    description = reader.GetString(8)
                });
            }

            return transactions;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT transactionID, walletID, categoryID, currencyID, 
                       transactionTime, transactionDate, transactionType, amount, description
                FROM Transactions
                WHERE transactionID = @TransactionId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TransactionId", transactionId);

            using var reader = await command.ExecuteReaderAsync(token);

            if (await reader.ReadAsync(token))
            {
                return new Transaction
                {
                    transactionID = reader.GetInt32(0),
                    walletID = reader.GetInt32(1),
                    categoryID = reader.GetInt32(2),
                    currencyID = reader.GetInt32(3),
                    transactionTime = TimeOnly.FromDateTime(reader.GetDateTime(4)),
                    transactionDate = DateOnly.FromDateTime(reader.GetDateTime(5)),
                    transactionType = reader.GetString(6),
                    amount = reader.GetDecimal(7),
                    description = reader.GetString(8)
                };
            }

            return null;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                INSERT INTO Transactions (walletID, categoryID, currencyID, transactionTime, transactionDate, transactionType, amount, description)
                VALUES (@WalletID, @CategoryID, @CurrencyID, @TransactionTime, @TransactionDate, @TransactionType, @Amount, @Description)
                RETURNING transactionID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@WalletID", transaction.walletID);
            command.Parameters.AddWithValue("@CategoryID", transaction.categoryID);
            command.Parameters.AddWithValue("@CurrencyID", transaction.currencyID);
            var dateTime = transaction.transactionDate.ToDateTime(transaction.transactionTime);
            command.Parameters.AddWithValue("@TransactionTime", dateTime);
            command.Parameters.AddWithValue("@TransactionDate", transaction.transactionDate);
            command.Parameters.AddWithValue("@TransactionType", transaction.transactionType);
            command.Parameters.AddWithValue("@Amount", transaction.amount);
            command.Parameters.AddWithValue("@Description", transaction.description);

            var transactionId = await command.ExecuteScalarAsync(token);
            transaction.transactionID = Convert.ToInt32(transactionId);

            return transaction;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                UPDATE Transactions
                SET walletID = @WalletID,
                    categoryID = @CategoryID,
                    currencyID = @CurrencyID,
                    transactionTime = @TransactionTime,
                    transactionDate = @TransactionDate,
                    transactionType = @TransactionType,
                    amount = @Amount,
                    description = @Description
                WHERE transactionID = @TransactionId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TransactionID", transaction.transactionID);
            command.Parameters.AddWithValue("@WalletID", transaction.walletID);
            command.Parameters.AddWithValue("@CategoryID", transaction.categoryID);
            command.Parameters.AddWithValue("@CurrencyID", transaction.currencyID);
            var dateTime = transaction.transactionDate.ToDateTime(transaction.transactionTime);
            command.Parameters.AddWithValue("@TransactionTime", dateTime);
            command.Parameters.AddWithValue("@TransactionDate", transaction.transactionDate);
            command.Parameters.AddWithValue("@TransactionType", transaction.transactionType);
            command.Parameters.AddWithValue("@Amount", transaction.amount);
            command.Parameters.AddWithValue("@Description", transaction.description);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM Transactions WHERE transactionID = @TransactionId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TransactionId", transactionId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
