using ErrorOr;

namespace ExpenseTracker.Domain.Errors.TransactionErrors
{
    public static class TransactionErrors
    {
        public static class NotFound
        {
            public static Error Transaction => Error.NotFound(
                code: "Transaction.NotFound",
                description: "Transaction with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error InvalidWalletId => Error.Validation(
                code: "Transaction.InvalidWalletId",
                description: "Wallet ID is not correct.");

            public static Error InvalidCategoryId => Error.Validation(
                code: "Transaction.InvalidCategoryId",
                description: "Category ID is not correct.");

            public static Error InvalidCurrencyId => Error.Validation(
                code: "Transaction.InvalidCurrencyId",
                description: "Currency ID is not correct.");

            public static Error InvalidTransactionType => Error.Validation(
                code: "Transaction.InvalidTransactionType",
                description: "Transaction type must be either 'Cash' or 'Card'.");

            public static Error DescriptionTooLong => Error.Validation(
                code: "Transaction.DescriptionTooLong",
                description: "Description cannot exceed 255 characters.");

            public static Error FutureDate => Error.Validation(
                code: "Transaction.FutureDate",
                description: "Transaction date cannot be in the future.");

            public static Error InvalidTransactionId => Error.Validation(
                code: "Transaction.InvalidTransactionId",
                description: "Transaction ID must be greater than zero.");
        }

        public static class Conflict
        {
            public static Error TransactionInUse => Error.Conflict(
                code: "Transaction.Conflict.InUse",
                description: "Cannot delete transaction as it is referenced by other records.");

            public static Error WalletMismatch => Error.Conflict(
                code: "Transaction.Conflict.WalletMismatch",
                description: "Transaction wallet does not match the expected wallet.");
        }
    }
}
