using ErrorOr;

namespace ExpenseTracker.Domain.Errors.DatabaseErrors
{
    public static class DatabaseErrors
    {
        public static class Database
        {
            public static Error ConnectionFailed => Error.Failure(
                code: "Transaction.Database.ConnectionFailed",
                description: "Failed to connect to the database.");

            public static Error OperationFailed => Error.Failure(
                code: "Transaction.Database.OperationFailed",
                description: "Database operation failed unexpectedly.");

            public static Error DuplicateTransaction => Error.Conflict(
                code: "Transaction.Database.DuplicateTransaction",
                description: "A transaction with these details already exists.");

            public static Error Timeout => Error.Failure(
                code: "Transaction.Database.Timeout",
                description: "Database operation timed out.");
        }
    }
}
