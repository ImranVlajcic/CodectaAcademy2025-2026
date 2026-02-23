using ErrorOr;
using ExpenseTracker.Application.TransactionFolders.Data;
using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure;
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
        public async Task<ErrorOr<AllTransactions>> GetTransactionsAsync(CancellationToken token)
        {
            var getTransactions = await _transactionRepository.GetTransactionsAsync(token);

            if (getTransactions.IsError)
            {
                return getTransactions.Errors;
            }

            return new AllTransactions
            {
                transactions = getTransactions.Value
            };
        }

        public async Task<ErrorOr<Transaction>> GetTransactionByIdAsync(int transactionId, CancellationToken token)
        {
            var validation = TransactionValidator.ValidateTransactionId(transactionId);

            if (validation.IsError) {
                return validation.Errors;
            }    

            var getTransaction = await _transactionRepository.GetTransactionByIdAsync(transactionId, token);

            if (getTransaction.IsError) {
                return getTransaction.Errors;            
            }
    
            return getTransaction;
        }

        public async Task<ErrorOr<Transaction>> CreateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            var validation = TransactionValidator.ValidateForCreate(transaction);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getTransaction = await _transactionRepository.CreateTransactionAsync(transaction, token);

            return getTransaction;
        }

        public async Task<ErrorOr<Updated>> UpdateTransactionAsync(Transaction transaction, CancellationToken token)
        {
            var validation = TransactionValidator.ValidateForUpdate(transaction);

            if (validation.IsError) {
                return validation.Errors;
            }

            var status = await _transactionRepository.UpdateTransactionAsync(transaction, token);

            if (status.IsError) {
                return status.Errors;
            }

            return status;
        }

        public async Task<ErrorOr<Deleted>> DeleteTransactionAsync(int transactionId, CancellationToken token)
        {
            var validation = TransactionValidator.ValidateTransactionId(transactionId);

            if (validation.IsError) {
                return validation.Errors;
            }

            var status = await _transactionRepository.DeleteTransactionAsync(transactionId, token);

            if (status.IsError) {
                return status.Errors;
            }

            return status;
        }
    }
}
