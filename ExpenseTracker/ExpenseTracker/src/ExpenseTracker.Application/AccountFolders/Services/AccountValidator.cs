using ErrorOr;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Domain.Errors.AccountErrors;
using System.Text.RegularExpressions;

namespace ExpenseTracker.Application.AccountFolders.Services
{
    public static class AccountValidator
    {
        private const int MaxUsernameLength = 50;
        private const int MaxEmailLength = 150;
        private const int MaxRealNameLength = 100;
        private const int MaxPhoneNumberLength = 30;

        public static ErrorOr<Success> ValidateForCreate(Account account)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(account.username))
            {
                errors.Add(AccountErrors.Validation.UsernameRequired);
            }
            else if (account.username.Length > MaxUsernameLength)
            {
                errors.Add(AccountErrors.Validation.UsernameTooLong);
            }

            if (string.IsNullOrWhiteSpace(account.email))
            {
                errors.Add(AccountErrors.Validation.EmailRequired);
            }
            else if (account.email.Length > MaxEmailLength)
            {
                errors.Add(AccountErrors.Validation.EmailTooLong);
            }
            else if (!IsValidEmail(account.email))
            {
                errors.Add(AccountErrors.Validation.InvalidEmail);
            }

            if (string.IsNullOrWhiteSpace(account.passwordHash))
            {
                errors.Add(AccountErrors.Validation.PasswordRequired);
            }

            if (string.IsNullOrWhiteSpace(account.realName))
            {
                errors.Add(AccountErrors.Validation.RealNameRequired);
            }
            else if (account.realName.Length > MaxRealNameLength)
            {
                errors.Add(AccountErrors.Validation.RealNameTooLong);
            }

            if (string.IsNullOrWhiteSpace(account.realSurname))
            {
                errors.Add(AccountErrors.Validation.RealSurnameRequired);
            }
            else if (account.realSurname.Length > MaxRealNameLength)
            {
                errors.Add(AccountErrors.Validation.RealSurnameTooLong);
            }

            if (!string.IsNullOrWhiteSpace(account.phoneNumber))
            {
                if (account.phoneNumber.Length > MaxPhoneNumberLength)
                {
                    errors.Add(AccountErrors.Validation.PhoneNumberTooLong);
                }
                else if (!IsValidPhoneNumber(account.phoneNumber))
                {
                    errors.Add(AccountErrors.Validation.InvalidPhoneNumber);
                }
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

        public static ErrorOr<Success> ValidateUserId(int userId)
        {
            if (userId <= 0)
            {
                return AccountErrors.Validation.InvalidUserId;
            }

            return Result.Success;
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^[\d\s\-\+\(\)]+$");
            return phoneRegex.IsMatch(phoneNumber) && phoneNumber.Length >= 10;
        }
    }
}
