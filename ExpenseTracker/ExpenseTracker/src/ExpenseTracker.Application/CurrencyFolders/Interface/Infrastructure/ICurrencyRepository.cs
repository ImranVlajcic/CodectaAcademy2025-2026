using ExpenseTracker.Domain.CurrencyData;
using ErrorOr;

namespace ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure
{
    public interface ICurrencyRepository
    {
        Task<ErrorOr<List<Currency>>> GetCurrenciesAsync(CancellationToken token);
        Task<ErrorOr<Currency>> GetCurrencyByIdAsync(int currencyId, CancellationToken token);
        Task<ErrorOr<Currency>> CreateCurrencyAsync(Currency currency, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateCurrencyAsync(Currency currency, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteCurrencyAsync(int currencyId, CancellationToken token);
    }
}
