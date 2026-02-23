using ExpenseTracker.Domain.TransactionData;
using ExpenseTracker.Application.TransactionFolders.Data;
using ErrorOr;

namespace ExpenseTracker.Application.TransactionFolders.Interface.Application
{
    public interface ITransactionService
    {
        Task<ErrorOr<AllTransactions>> GetTransactionsAsync(CancellationToken token);
        Task<ErrorOr<Transaction>> GetTransactionByIdAsync(int transactionId, CancellationToken token);
        Task<ErrorOr<Transaction>> CreateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteTransactionAsync(int transactionId, CancellationToken token);
    }
}
