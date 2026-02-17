using ExpenseTracker.Domain.CurrencyData;
using ExpenseTracker.Application.CurrencyFolders.Data;

namespace ExpenseTracker.Application.CurrencyFolders.Interface.Application
{
    public interface ICurrencyService
    {
        Task<AllCurrencies> GetCurrenciesAsync(CancellationToken token);
        Task<Currency> GetCurrencyByIdAsync(int currencyId, CancellationToken token);
        Task<Currency> CreateCurrencyAsync(Currency currency, CancellationToken token);
        Task<bool> UpdateCurrencyAsync(Currency currency, CancellationToken token);
        Task<bool> DeleteCurrencyAsync(int currencyId, CancellationToken token);
    }
}
