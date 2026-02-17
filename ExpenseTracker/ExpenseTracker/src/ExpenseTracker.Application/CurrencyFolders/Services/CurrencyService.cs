using ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure;
using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using ExpenseTracker.Application.CurrencyFolders.Data;
using ExpenseTracker.Domain.CurrencyData;

namespace ExpenseTracker.Application.CurrencyFolders.Services

{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        public async Task<AllCurrencies> GetCurrenciesAsync(CancellationToken token)
        {
            var getCurrencies = await _currencyRepository.GetCurrenciesAsync(token);

            return new AllCurrencies
            {
                currencies = getCurrencies
            };
        }

        public async Task<Currency> GetCurrencyByIdAsync(int currencyId, CancellationToken token)
        {
            var getCurrency = await _currencyRepository.GetCurrencyByIdAsync(currencyId, token);

            return getCurrency;
        }

        public async Task<Currency> CreateCurrencyAsync(Currency currency, CancellationToken token)
        {
            var getCurrency = await _currencyRepository.CreateCurrencyAsync(currency, token);

            return getCurrency;
        }

        public async Task<bool> UpdateCurrencyAsync(Currency currency, CancellationToken token)
        {
            var status = await _currencyRepository.UpdateCurrencyAsync(currency, token);

            return status;
        }

        public async Task<bool> DeleteCurrencyAsync(int currencyId, CancellationToken token)
        {
            var status = await _currencyRepository.DeleteCurrencyAsync(currencyId, token);

            return status;
        }
    }
}
