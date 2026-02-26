using ErrorOr;
using ExpenseTracker.Domain.Errors.CategoryErrors;
using ErrorOr;
using ExpenseTracker.Domain.CategoryData;

namespace ExpenseTracker.Application.CategoryFolders.Services
{
    public static class CategoryValidator
    {
        private const int MaxNameLength = 100;
        private static readonly string[] ValidCategoryTypes = { "Income", "Expense" };

        public static ErrorOr<Success> ValidateForCreate(Category category)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(category.reason) ||
                !ValidCategoryTypes.Contains(category.reason, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add(CategoryErrors.Validation.InvalidCategoryType);
            }

            if (!string.IsNullOrWhiteSpace(category.categoryName) ||
                category.categoryName.Length > MaxNameLength)
            {
                errors.Add(CategoryErrors.Validation.CategoryNameTooLong);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(Category category)
        {
            var errors = new List<Error>();

            if (category.categoryID <= 0)
            {
                errors.Add(CategoryErrors.Validation.InvalidCategoryId);
            }

            var createValidation = ValidateForCreate(category);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateCategoryId(int categoryId)
        {
            if (categoryId <= 0)
            {
                return CategoryErrors.Validation.InvalidCategoryId;
            }

            return Result.Success;
        }
    }
}
