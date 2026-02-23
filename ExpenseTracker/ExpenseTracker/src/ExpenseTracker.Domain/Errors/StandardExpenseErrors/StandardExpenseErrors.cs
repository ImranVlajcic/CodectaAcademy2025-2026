using ErrorOr;

namespace ExpenseTracker.Domain.Errors.StandardExpenseErrors
{
    public static class StandardExpenseErrors
    {
        public static class NotFound
        {
            public static Error StandardExpense => Error.NotFound(
                code: "StandardExpense.NotFound",
                description: "Standard Exepnse with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error InvalidWalletId => Error.Validation(
                code: "StandardExpense.InvalidWalletId",
                description: "Wallet ID is not correct.");

            public static Error ReasonTooLong => Error.Validation(
                code: "StandardExpense.ReasonTooLong",
                description: "Reason cannot exceed 30 characters.");

            public static Error DescriptionTooLong => Error.Validation(
                code: "StandardExpense.DescriptionTooLong",
                description: "Description cannot exceed 256 characters.");

            public static Error InvalidFrequencyType => Error.Validation(
                code: "StandardExpense.InvalidFrequencyType",
                description: "Frequency type must be either 'Daily', 'Weekly','Monthly' or 'Yearly'.");

            public static Error InvalidStandardExpenseId => Error.Validation(
                code: "StandardExpense.InvalidStandardExpenseId",
                description: "StandardExpense ID must be greater than zero.");

            public static Error PastDate => Error.Validation(
                code: "StandardExpensen.PastDate",
                description: "Next payment date cannot be in the past.");
        }

        public static class Conflict
        {
            public static Error TransactionInUse => Error.Conflict(
                code: "StandardExpense.Conflict.InUse",
                description: "Cannot delete standard expense as it is referenced by other records.");

            public static Error WalletMismatch => Error.Conflict(
                code: "StandardExpense.Conflict.WalletMismatch",
                description: "StandardExpense wallet does not match the expected wallet.");
        }
    }
}
