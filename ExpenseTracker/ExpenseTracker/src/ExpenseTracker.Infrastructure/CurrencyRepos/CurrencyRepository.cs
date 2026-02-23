using ErrorOr;
using ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.CurrencyData;
using ExpenseTracker.Domain.Errors.CurrencyErrors;
using ExpenseTracker.Domain.Errors.DatabaseErrors;
using ExpenseTracker.Domain.Errors.TransactionErrors;
using ExpenseTracker.Infrastructure.CurrencyRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.CurrencyRepos
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyOptions _options;
        const string ForeignKeyViolation = "20503";
        const string UniqueViolation = "20505";

        public CurrencyRepository(CurrencyOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<ErrorOr<List<Currency>>> GetCurrenciesAsync(CancellationToken token)
        {

            var currencies = new List<Currency>();

            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT currencyID, currencyCode, currencyName, rateToEuro 
                FROM Currency
                ORDER BY currencyID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
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

        public async Task<ErrorOr<Currency>> GetCurrencyByIdAsync(int currencyId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT currencyID, currencyCode, currencyName, rateToEuro 
                FROM Currency
                WHERE currencyID = @CurrencyId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
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

                return CurrencyErrors.NotFound.Currency;
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

        public async Task<ErrorOr<Currency>> CreateCurrencyAsync(Currency currency, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                INSERT INTO Currency (currencyCode, currencyName, rateToEuro)
                VALUES (@CurrencyCode, @CurrencyName, @RateToEuro)
                RETURNING currencyID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CurrencyCode", currency.currencyCode);
                command.Parameters.AddWithValue("@CurrencyName", currency.currencyName);
                command.Parameters.AddWithValue("@RateToEuro", currency.rateToEuro);

                var currencyId = await command.ExecuteScalarAsync(token);
                if (currencyId == null)
                {
                    return DatabaseErrors.Database.OperationFailed;
                }
                currency.currencyID = Convert.ToInt32(currencyId);

                return currency;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
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

        public async Task<ErrorOr<Updated>> UpdateCurrencyAsync(Currency currency, CancellationToken token)
        {
            try
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
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CurrencyId", currency.currencyID);
                command.Parameters.AddWithValue("@CurrencyCode", currency.currencyCode);
                command.Parameters.AddWithValue("@CurrencyName", currency.currencyName);
                command.Parameters.AddWithValue("@RateToEuro", currency.rateToEuro);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);

                if (rowsAffected == 0)
                {
                    return CurrencyErrors.NotFound.Currency;
                }

                return Result.Updated;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
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

        public async Task<ErrorOr<Deleted>> DeleteCurrencyAsync(int currencyId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "DELETE FROM Currency WHERE currencyID = @CurrencyId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CurrencyId", currencyId);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);
                if (rowsAffected == 0)
                {
                    return CurrencyErrors.NotFound.Currency;
                }

                return Result.Deleted;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return CurrencyErrors.Conflict.CurrencyInUse;
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
}
