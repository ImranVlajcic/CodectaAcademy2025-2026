using ExpenseTracker.Domain.TransactionData;


namespace ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsAsync(CancellationToken token);
        Task<Transaction?> GetTransactionByIdAsync(int transactionId, CancellationToken token);
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken token);
        Task<bool> DeleteTransactionAsync(int transactionId, CancellationToken token);
    }
}
