using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Infrastructure.AccountRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.AccountRepos

{
    internal class AccountRepository : IAccountRepository
    {
        private readonly AccountOptions _options;

        public AccountRepository(AccountOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<Account>> GetAccountsAsync(CancellationToken token)
        {

            var accounts = new List<Account>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT userID, username, email, accPassword, 
                       realName, realSurname, phoneNumber
                FROM Account
                ORDER BY userID";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
            {
                accounts.Add(new Account
                {
                    userID = reader.GetInt32(0),
                    username = reader.GetString(1),
                    email = reader.GetString(2),
                    accPassword = reader.GetString(3),
                    realName = reader.GetString(4),
                    realSurname = reader.GetString(5),
                    phoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6)
                });
            }

            return accounts;
        }

        public async Task<Account?> GetAccountByIdAsync(int userId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT userID, username, email, accPassword, 
                       realName, realSurname, phoneNumber
                FROM Account
                WHERE userID = @UserId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync(token);

            if (await reader.ReadAsync(token))
            {
                return new Account
                {
                    userID = reader.GetInt32(0),
                    username = reader.GetString(1),
                    email = reader.GetString(2),
                    accPassword = reader.GetString(3),
                    realName = reader.GetString(4),
                    realSurname = reader.GetString(5),
                    phoneNumber = reader.GetString(6)
                };
            }

            return null;
        }

        public async Task<Account> CreateAccountAsync(Account account, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                INSERT INTO Account (username, email, accPassword, realName, realSurname, phoneNumber)
                VALUES (@Username, @Email, @Password, @RealName, @RealSurname, @PhoneNumber)
                RETURNING userID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", account.username);
            command.Parameters.AddWithValue("@Email", account.email);
            command.Parameters.AddWithValue("@Password", account.accPassword);
            command.Parameters.AddWithValue("@RealName", account.realName);
            command.Parameters.AddWithValue("@RealSurname", account.realSurname);
            command.Parameters.AddWithValue("@PhoneNumber", account.phoneNumber);

            var userId = await command.ExecuteScalarAsync(token);
            account.userID = Convert.ToInt32(userId);

            return account;
        }

        public async Task<bool> UpdateAccountAsync(Account account, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                UPDATE Account
                SET username = @Username,
                    email = @Email,
                    accPassword = @Password,
                    realName = @RealName,
                    realSurname = @RealSurname,
                    phoneNumber = @PhoneNumber
                WHERE userID = @UserId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", account.userID);
            command.Parameters.AddWithValue("@Username", account.username);
            command.Parameters.AddWithValue("@Email", account.email);
            command.Parameters.AddWithValue("@Password", account.accPassword);
            command.Parameters.AddWithValue("@RealName", account.realName);
            command.Parameters.AddWithValue("@RealSurname", account.realSurname);
            command.Parameters.AddWithValue("@PhoneNumber", account.phoneNumber );

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAccountAsync(int userId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM Account WHERE userID = @UserId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
