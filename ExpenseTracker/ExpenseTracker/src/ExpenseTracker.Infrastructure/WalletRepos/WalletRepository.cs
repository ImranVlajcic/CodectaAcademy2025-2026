using ExpenseTracker.Application.WalletFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.WalletData;
using ExpenseTracker.Infrastructure.WalletRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.WalletRepos
{
    internal class WalletRepository : IWalletRepository
    {
        private readonly WalletOptions _options;

        public WalletRepository(WalletOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<Wallet>> GetWalletsAsync(CancellationToken token)
        {

            var wallets = new List<Wallet>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT walletID, userID, currencyID, balance, purpose
                FROM Wallet
                ORDER BY walletID";

            using var command = new NpgsqlCommand(sql, connection);
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

        public async Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT walletID, userID, currencyID, balance, purpose
                FROM Wallet
                WHERE walletID = @WalletId";

            using var command = new NpgsqlCommand(sql, connection);
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

            return null;
        }

        public async Task<Wallet> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                INSERT INTO Wallet (userID, currencyID, balance, purpose)
                VALUES (@UserID, @CurrencyID, @Balance, @Purpose )
                RETURNING userID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserID", wallet.userID);
            command.Parameters.AddWithValue("@CurrencyID", wallet.currencyID);
            command.Parameters.AddWithValue("@Balance", wallet.balance);
            command.Parameters.AddWithValue("@Purpose", wallet.purpose);

            var walletId = await command.ExecuteScalarAsync(token);
            wallet.walletID = Convert.ToInt32(walletId);

            return wallet;
        }

        public async Task<bool> UpdateWalletAsync(Wallet wallet, CancellationToken token)
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
            command.Parameters.AddWithValue("@WalletID", wallet.walletID);
            command.Parameters.AddWithValue("@UserID", wallet.userID);
            command.Parameters.AddWithValue("@CurrencyID", wallet.currencyID);
            command.Parameters.AddWithValue("@Balance", wallet.balance);
            command.Parameters.AddWithValue("@Purpose", wallet.purpose);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteWalletAsync(int walletId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM Wallet WHERE walletID = @WalletId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@WalletId", walletId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
