using ExpenseTracker.Domain.CurrencyData;
using ExpenseTracker.Application.CurrencyFolders.Data;
using ErrorOr;

namespace ExpenseTracker.Application.CurrencyFolders.Interface.Application
{
    public interface ICurrencyService
    {
        Task<ErrorOr<AllCurrencies>> GetCurrenciesAsync(CancellationToken token);
        Task<ErrorOr<Currency>> GetCurrencyByIdAsync(int currencyId, CancellationToken token);
        Task<ErrorOr<Currency>> CreateCurrencyAsync(Currency currency, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateCurrencyAsync(Currency currency, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteCurrencyAsync(int currencyId, CancellationToken token);
    }
}
