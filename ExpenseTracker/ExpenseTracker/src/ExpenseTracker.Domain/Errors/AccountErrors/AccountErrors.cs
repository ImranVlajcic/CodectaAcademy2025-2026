using ErrorOr;

namespace ExpenseTracker.Domain.Errors.AccountErrors
{
    public static class AccountErrors
    {
        public static class NotFound
        {
            public static Error Account => Error.NotFound(
                code: "Account.NotFound",
                description: "Account with the specified ID was not found.");
        }

        public static class Validation
        {
            public static Error InvalidUserId => Error.Validation(
                code: "Account.InvalidUserId",
                description: "User ID must be greater than zero.");

            public static Error UsernameRequired => Error.Validation(
                code: "Account.UsernameRequired",
                description: "Username is required.");

            public static Error EmailRequired => Error.Validation(
                code: "Account.EmailRequired",
                description: "Email is required.");

            public static Error InvalidEmail => Error.Validation(
                code: "Account.InvalidEmail",
                description: "Invalid email format.");

            public static Error PasswordRequired => Error.Validation(
                code: "Account.PasswordRequired",
                description: "Password is required.");

            public static Error WeakPassword => Error.Validation(
                code: "Account.WeakPassword",
                description: "Password must be at least 8 characters long and contain uppercase, lowercase, number, and special character.");

            public static Error RealNameRequired => Error.Validation(
                code: "Account.RealNameRequired",
                description: "Real name is required.");

            public static Error RealSurnameRequired => Error.Validation(
                code: "Account.RealSurnameRequired",
                description: "Real surname is required.");

            public static Error InvalidPhoneNumber => Error.Validation(
                code: "Account.InvalidPhoneNumber",
                description: "Invalid phone number format.");

            public static Error InvalidCredentials => Error.Validation(
                code: "Account.InvalidCredentials",
                description: "Invalid email or password.");

            public static Error AccountInactive => Error.Failure(
                code: "Account.AccountInactive",
                description: "Account is inactive. Please contact support.");

            public static Error InvalidRefreshToken => Error.Unauthorized(
                code: "Account.InvalidRefreshToken",
                description: "Invalid or expired refresh token.");

            public static Error UsernameTooLong => Error.Validation(
                code: "Account.UsernameTooLong",
                description: "Username cannot exceed 50 characters.");

            public static Error EmailTooLong => Error.Validation(
                code: "Account.EmailTooLong",
                description: "Email cannot exceed 150 characters.");

            public static Error RealNameTooLong => Error.Validation(
                code: "Account.RealNameTooLong",
                description: "Real name cannot exceed 100 characters.");

            public static Error RealSurnameTooLong => Error.Validation(
                code: "Account.RealSurnameTooLong",
                description: "Real surname cannot exceed 100 characters.");

            public static Error PhoneNumberTooLong => Error.Validation(
                code: "Account.PhoneNumberTooLong",
                description: "Phone number cannot exceed 30 characters.");
        }

        public static class Database
        {
            public static Error DuplicateEmail => Error.Conflict(
                code: "Account.Database.DuplicateEmail",
                description: "An account with this email already exists.");

            public static Error DuplicateUsername => Error.Conflict(
                code: "Account.Database.DuplicateUsername",
                description: "An account with this username already exists.");
        }

        public static class Conflict
        {
            public static Error AccountInUse => Error.Conflict(
                code: "Account.Conflict.InUse",
                description: "Cannot delete account as it has associated transactions.");
        }
    }
}
