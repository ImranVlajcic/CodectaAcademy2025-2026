using ErrorOr;
using ExpenseTracker.Application.CurrencyFolders.Data;
using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure;
using ExpenseTracker.Application.CurrencyFolders.Services;
using ExpenseTracker.Application.TransactionFolders.Services;
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
        public async Task<ErrorOr<AllCurrencies>> GetCurrenciesAsync(CancellationToken token)
        {
            var getCurrencies = await _currencyRepository.GetCurrenciesAsync(token);

            if (getCurrencies.IsError)
            {
                return getCurrencies.Errors;
            }

            return new AllCurrencies
            {
                currencies = getCurrencies.Value
            };
        }

        public async Task<ErrorOr<Currency>> GetCurrencyByIdAsync(int currencyId, CancellationToken token)
        {
            var validation = CurrencyValidator.ValidateCurrencyId(currencyId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getCurrency = await _currencyRepository.GetCurrencyByIdAsync(currencyId, token);

            if (getCurrency.IsError) {
                return getCurrency.Errors;            
            }

            return getCurrency;
        }

        public async Task<ErrorOr<Currency>> CreateCurrencyAsync(Currency currency, CancellationToken token)
        {
            var validation = CurrencyValidator.ValidateForCreate(currency);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getCurrency = await _currencyRepository.CreateCurrencyAsync(currency, token);

            return getCurrency;
        }

        public async Task<ErrorOr<Updated>> UpdateCurrencyAsync(Currency currency, CancellationToken token)
        {
            var validation = CurrencyValidator.ValidateForUpdate(currency);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _currencyRepository.UpdateCurrencyAsync(currency, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }

        public async Task<ErrorOr<Deleted>> DeleteCurrencyAsync(int currencyId, CancellationToken token)
        {
            var validation = CurrencyValidator.ValidateCurrencyId(currencyId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _currencyRepository.DeleteCurrencyAsync(currencyId, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }
    }
}
