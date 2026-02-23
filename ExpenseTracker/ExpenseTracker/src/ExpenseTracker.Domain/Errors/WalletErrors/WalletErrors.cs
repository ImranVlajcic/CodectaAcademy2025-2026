using ErrorOr;

namespace ExpenseTracker.Domain.Errors.WalletErrors
{
    public static class WalletErrors
    {
        public static class NotFound
        {
            public static Error Wallet => Error.NotFound(
                code: "Wallet.NotFound",
                description: "Wallet with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error InvalidUserId => Error.Validation(
                code: "Wallet.InvalidUserId",
                description: "User ID is not correct.");

            public static Error InvalidCurrencyId => Error.Validation(
                code: "Wallet.InvalidCurrencyId",
                description: "Currency ID is not correct.");

            public static Error PurposeTooLong => Error.Validation(
                code: "Wallet.PurposeTooLong",
                description: "Purpose cannot exceed 100 characters.");

            public static Error InvalidWalletId => Error.Validation(
                code: "Wallet.InvalidWalletId",
                description: "Wallet ID must be greater than zero.");
        }

        public static class Conflict
        {
            public static Error WalletInUse => Error.Conflict(
                code: "Wallet.Conflict.InUse",
                description: "Cannot delete wallet as it is referenced by other records.");

            public static Error UserMismatch => Error.Conflict(
                code: "Wallet.Conflict.UserMismatch",
                description: "Wallet user does not match the expected user.");
        }
    }
}
