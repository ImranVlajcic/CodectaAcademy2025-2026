using ErrorOr;

namespace ExpenseTracker.Domain.Errors.CategoryErrors
{
    public static class CategoryErrors
    {
        public static class NotFound
        {
            public static Error Category => Error.NotFound(
                code: "Category.NotFound",
                description: "Category with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error InvalidCategoryType => Error.Validation(
                code: "Category.InvalidCategoryType",
                description: "Transaction type must be either 'Income' or 'Expense'.");

            public static Error CategoryNameTooLong => Error.Validation(
                code: "Category.CategoryNameTooLong",
                description: "Category name cannot exceed 100 characters.");

            public static Error InvalidCategoryId => Error.Validation(
                code: "Category.InvalidCategoryId",
                description: "Category ID must be greater than zero.");
        }

        public static class Conflict
        {
            public static Error CategoryInUse => Error.Conflict(
                code: "Category.Conflict.InUse",
                description: "Cannot delete category as it is referenced by other records.");
        }
    }
}
