using ExpenseTracker.Domain.TransactionData;
using ExpenseTracker.Application.TransactionFolders.Data;

namespace ExpenseTracker.Application.TransactionFolders.Interface.Application
{
    public interface ITransactionService
    {
        Task<AllTransactions> GetTransactionsAsync(CancellationToken token);
        Task<Transaction> GetTransactionByIdAsync(int transactionId, CancellationToken token);
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<bool> DeleteTransactionAsync(int transactionId, CancellationToken token);
    }
}
