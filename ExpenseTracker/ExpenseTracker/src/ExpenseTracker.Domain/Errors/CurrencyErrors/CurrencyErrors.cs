using ErrorOr;

namespace ExpenseTracker.Domain.Errors.CurrencyErrors
{
    public static class CurrencyErrors
    {
        public static class NotFound
        {
            public static Error Currency => Error.NotFound(
                code: "Currency.NotFound",
                description: "Currency with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error CurrencyCodeTooLong => Error.Validation(
                code: "Currency.CorrencyCodeTooLong",
                description: "Currency code cannot exceed 3 characters.");

            public static Error CurrencyNameTooLong => Error.Validation(
                code: "Currency.CorrencyNameTooLong",
                description: "Currency name cannot exceed 50 characters.");

            public static Error InvalidCurrencyId => Error.Validation(
                code: "Currency.InvalidCurrencyId",
                description: "Currency ID must be greater than zero.");
        }

        public static class Conflict
        {
            public static Error CurrencyInUse => Error.Conflict(
                code: "Currency.Conflict.InUse",
                description: "Cannot delete currency as it is referenced by other records.");
        }
    }
}
