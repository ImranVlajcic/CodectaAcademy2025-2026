using ErrorOr;

namespace ExpenseTracker.Domain.Errors.DatabaseErrors
{
    public static class DatabaseErrors
    {
        public static class Database
        {
            public static Error ConnectionFailed => Error.Failure(
                code: "Database.ConnectionFailed",
                description: "Failed to connect to the database.");

            public static Error OperationFailed => Error.Failure(
                code: "Database.OperationFailed",
                description: "Database operation failed unexpectedly.");

            public static Error DuplicateRow => Error.Conflict(
                code: "Database.DuplicateTransaction",
                description: "A duplicate with these details already exists.");

            public static Error Timeout => Error.Failure(
                code: "Database.Timeout",
                description: "Database operation timed out.");
        }
    }
}
