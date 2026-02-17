using ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.CurrencyData;
using ExpenseTracker.Infrastructure.CurrencyRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.CurrencyRepos
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyOptions _options;

        public CurrencyRepository(CurrencyOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<Currency>> GetCurrenciesAsync(CancellationToken token)
        {

            var currencies = new List<Currency>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT currencyID, currencyCode, currencyName, rateToEuro 
                FROM Currency
                ORDER BY currencyID";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
            {
                currencies.Add(new Currency
                {
                    currencyID = reader.GetInt32(0),
                    currencyCode = reader.GetString(1),
                    currencyName = reader.GetString(2),
                    rateToEuro = reader.GetDecimal(3)
                });
            }

            return currencies;
        }

        public async Task<Currency?> GetCurrencyByIdAsync(int currencyId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT currencyID, currencyCode, currencyName, rateToEuro 
                FROM Currency
                WHERE currencyID = @CurrencyId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CurrencyId", currencyId);

            using var reader = await command.ExecuteReaderAsync(token);

            if (await reader.ReadAsync(token))
            {
                return new Currency
                {
                    currencyID = reader.GetInt32(0),
                    currencyCode = reader.GetString(1),
                    currencyName = reader.GetString(2),
                    rateToEuro = reader.GetDecimal(3)
                };
            }

            return null;
        }

        public async Task<Currency> CreateCurrencyAsync(Currency currency, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                INSERT INTO Currency (currencyCode, currencyName, rateToEuro)
                VALUES (@CurrencyCode, @CurrencyName, @RateToEuro)
                RETURNING currencyID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CurrencyCode", currency.currencyCode);
            command.Parameters.AddWithValue("@CurrencyName", currency.currencyName);
            command.Parameters.AddWithValue("@RateToEuro", currency.rateToEuro);

            var userId = await command.ExecuteScalarAsync(token);
            currency.currencyID = Convert.ToInt32(userId);

            return currency;
        }

        public async Task<bool> UpdateCurrencyAsync(Currency currency, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                UPDATE Currency
                SET currencyCode = @CurrencyCode,
                    currencyName = @CurrencyName,
                    rateToEuro = @RateToEuro
                WHERE currencyID = @CurrencyId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CurrencyId", currency.currencyID);
            command.Parameters.AddWithValue("@CurrencyCode", currency.currencyCode);
            command.Parameters.AddWithValue("@CurrencyName", currency.currencyName);
            command.Parameters.AddWithValue("@RateToEuro", currency.rateToEuro);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCurrencyAsync(int currencyId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM Currency WHERE currencyID = @CurrencyId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CurrencyId", currencyId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
