using ErrorOr;
using ExpenseTracker.Domain.Errors.TransactionErrors;
using ExpenseTracker.Domain.TransactionData;

namespace ExpenseTracker.Application.TransactionFolders.Services
{
    public static class TransactionValidator
    {
        private const int MaxDescriptionLength = 255;
        private static readonly string[] ValidTransactionTypes = { "Cash", "Card" };

        public static ErrorOr<Success> ValidateForCreate(Transaction transaction)
        {
            var errors = new List<Error>();

            if (transaction.walletID <= 0)
            {
                errors.Add(TransactionErrors.Validation.InvalidWalletId);
            }

            if (transaction.categoryID <= 0)
            {
                errors.Add(TransactionErrors.Validation.InvalidCategoryId);
            }

            if (transaction.currencyID <= 0)
            {
                errors.Add(TransactionErrors.Validation.InvalidCurrencyId);
            }

            if (string.IsNullOrWhiteSpace(transaction.transactionType) ||
                !ValidTransactionTypes.Contains(transaction.transactionType, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add(TransactionErrors.Validation.InvalidTransactionType);
            }

            if (!string.IsNullOrWhiteSpace(transaction.description) &&
                transaction.description.Length > MaxDescriptionLength)
            {
                errors.Add(TransactionErrors.Validation.DescriptionTooLong);
            }

            if (transaction.transactionDate > DateOnly.FromDateTime(DateTime.Today))
            {
                errors.Add(TransactionErrors.Validation.FutureDate);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(Transaction transaction)
        {
            var errors = new List<Error>();

            if (transaction.transactionID <= 0)
            {
                errors.Add(TransactionErrors.Validation.InvalidTransactionId);
            }

            var createValidation = ValidateForCreate(transaction);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateTransactionId(int transactionId)
        {
            if (transactionId <= 0)
            {
                return TransactionErrors.Validation.InvalidTransactionId;
            }

            return Result.Success;
        }
    }
}
