using ErrorOr;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Domain.Errors.AccountErrors;

namespace ExpenseTracker.Application.AccountFolders.Services
{
    public static class AccountValidator
    {
        private const int MaxUsernameLength = 50;
        private const int MaxEmailLength = 150;
        private static readonly string[] ValidTransactionTypes = { "Cash", "Card" };

        public static ErrorOr<Success> ValidateForCreate(Account account)
        {
            var errors = new List<Error>();

            if (!string.IsNullOrWhiteSpace(account.username) &&
                account.email.Length > MaxUsernameLength)
            {
                errors.Add(AccountErrors.Validation.UsernameRequired);
            }

            if (!string.IsNullOrWhiteSpace(account.email) &&
                account.email.Length > MaxEmailLength)
            {
                errors.Add(AccountErrors.Validation.EmailRequired);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(Account account)
        {
            var errors = new List<Error>();

            if (account.userID <= 0)
            {
                errors.Add(AccountErrors.Validation.InvalidUserId);
            }

            var createValidation = ValidateForCreate(account);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateUserId(int transactionId)
        {
            if (transactionId <= 0)
            {
                return AccountErrors.Validation.InvalidUserId;
            }

            return Result.Success;
        }
    }
}
