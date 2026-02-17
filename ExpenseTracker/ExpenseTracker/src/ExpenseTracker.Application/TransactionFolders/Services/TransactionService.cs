using ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure;
using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using ExpenseTracker.Application.TransactionFolders.Data;
using ExpenseTracker.Domain.TransactionData;

namespace ExpenseTracker.Application.TransactionFolders.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<AllTransactions> GetTransactionsAsync(CancellationToken token)
        {
            var getTransactions = await _transactionRepository.GetTransactionsAsync(token);

            return new AllTransactions
            {
                transactions = getTransactions
            };
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId, CancellationToken token)
        {
            var getTransaction = await _transactionRepository.GetTransactionByIdAsync(transactionId, token);

            return getTransaction;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            var getTransaction = await _transactionRepository.CreateTransactionAsync(transaction, token);

            return getTransaction;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            var status = await _transactionRepository.UpdateTransactionAsync(transaction, token);

            return status;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId, CancellationToken token)
        {
            var status = await _transactionRepository.DeleteTransactionAsync(transactionId, token);

            return status;
        }
    }
}
