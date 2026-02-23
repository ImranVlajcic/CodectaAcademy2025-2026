using ErrorOr;
using ExpenseTracker.Domain.Errors.StandardExpenseErrors;
using ExpenseTracker.Domain.StandardExpenseData;

namespace ExpenseTracker.Application.StandardExpenseFolders.Services
{
    public static class StandardExpenseValidator
    {
        private const int MaxDescriptionLength = 255;
        private const int MaxReasonLength = 30;
        private static readonly string[] ValidFrequencyTypes = { "Daily", "Weekly", "Monthly", "Yearly"};

        public static ErrorOr<Success> ValidateForCreate(StandardExpense standardexpense)
        {
            var errors = new List<Error>();

            if (standardexpense.walletID <= 0)
            {
                errors.Add(StandardExpenseErrors.Validation.InvalidWalletId);
            }

            if (string.IsNullOrWhiteSpace(standardexpense.frequency) ||
                !ValidFrequencyTypes.Contains(standardexpense.frequency, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add(StandardExpenseErrors.Validation.InvalidFrequencyType);
            }

            if (!string.IsNullOrWhiteSpace(standardexpense.reason) &&
                standardexpense.reason.Length > MaxReasonLength)
            {
                errors.Add(StandardExpenseErrors.Validation.ReasonTooLong);
            }

            if (!string.IsNullOrWhiteSpace(standardexpense.description) &&
                standardexpense.description.Length > MaxDescriptionLength)
            {
                errors.Add(StandardExpenseErrors.Validation.DescriptionTooLong);
            }

            if (standardexpense.nextDate < DateOnly.FromDateTime(DateTime.Today))
            {
                errors.Add(StandardExpenseErrors.Validation.PastDate);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(StandardExpense standardexpense)
        {
            var errors = new List<Error>();

            if (standardexpense.expenseID <= 0)
            {
                errors.Add(StandardExpenseErrors.Validation.InvalidStandardExpenseId);
            }

            var createValidation = ValidateForCreate(standardexpense);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateStandardExpenseId(int expenseId)
        {
            if (expenseId <= 0)
            {
                return StandardExpenseErrors.Validation.InvalidStandardExpenseId;
            }

            return Result.Success;
        }
    }
}
