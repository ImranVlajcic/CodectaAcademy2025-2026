using ExpenseTracker.Domain.TransactionData;
using ErrorOr;

namespace ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure
{
    public interface ITransactionRepository
    {
        Task<ErrorOr<List<Transaction>>> GetTransactionsAsync(CancellationToken token);
        Task<ErrorOr<Transaction>> GetTransactionByIdAsync(int transactionId, CancellationToken token);
        Task<ErrorOr<Transaction>> CreateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteTransactionAsync(int transactionId, CancellationToken token);
    }
}
