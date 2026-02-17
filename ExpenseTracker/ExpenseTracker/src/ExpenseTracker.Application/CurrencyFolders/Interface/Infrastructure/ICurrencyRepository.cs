using ExpenseTracker.Domain.CurrencyData;

namespace ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetCurrenciesAsync(CancellationToken token);
        Task<Currency?> GetCurrencyByIdAsync(int currencyId, CancellationToken token);
        Task<Currency> CreateCurrencyAsync(Currency currency, CancellationToken token);
        Task<bool> UpdateCurrencyAsync(Currency currency, CancellationToken token);
        Task<bool> DeleteCurrencyAsync(int currencyId, CancellationToken token);
    }
}
