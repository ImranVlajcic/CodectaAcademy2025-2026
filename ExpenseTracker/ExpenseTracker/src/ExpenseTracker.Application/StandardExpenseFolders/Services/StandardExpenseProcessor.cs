using ErrorOr;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
using ExpenseTracker.Application.WalletFolders.Interface.Infrastructure;
using ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.StandardExpenseData;
using ExpenseTracker.Domain.TransactionData;
using ExpenseTracker.Domain.WalletData;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.StandardExpenseFolders.Services
{
    public class StandardExpenseProcessor
    {
        private readonly IStandardExpenseRepository _standardExpenseRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<StandardExpenseProcessor> _logger;

        public StandardExpenseProcessor(
            IStandardExpenseRepository standardExpenseRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            ILogger<StandardExpenseProcessor> logger)
        {
            _standardExpenseRepository = standardExpenseRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<int>> ProcessDueExpensesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("========== PROCESSING DUE EXPENSES ==========");
            _logger.LogInformation("Current DateTime.Today: {Today}", DateTime.Today);
            _logger.LogInformation("Current DateTime.UtcNow: {UtcNow}", DateTime.UtcNow);

            var queryDate = DateOnly.FromDateTime(DateTime.Today);
            _logger.LogInformation("Query date (DateOnly): {QueryDate}", queryDate);

            try
            {
                var dueExpensesResult = await _standardExpenseRepository.GetDueExpensesAsync(
                    queryDate,
                    cancellationToken);

                if (dueExpensesResult.IsError)
                {
                    _logger.LogError("Failed to fetch due expenses: {Errors}",
                        string.Join(", ", dueExpensesResult.Errors.Select(e => e.Description)));
                    return dueExpensesResult.Errors;
                }

                var dueExpenses = dueExpensesResult.Value;
                _logger.LogInformation("Found {Count} due standard expenses", dueExpenses.Count);

                if (dueExpenses.Count > 0)
                {
                    foreach (var exp in dueExpenses)
                    {
                        _logger.LogInformation("  - Expense ID: {ExpenseId}, Wallet: {WalletId}, NextDate: {NextDate}, Amount: {Amount}",
                            exp.expenseID, exp.walletID, exp.nextDate, exp.amount);
                    }
                }
                else
                {
                    _logger.LogWarning("NO EXPENSES FOUND! Query returned empty list.");
                }

                int processedCount = 0;

                foreach (var expense in dueExpenses)
                {
                    var result = await ProcessSingleExpenseAsync(expense, cancellationToken);

                    if (result.IsError)
                    {
                        _logger.LogError("Failed to process standard expense {ExpenseId}: {Errors}",
                            expense.expenseID, string.Join(", ", result.Errors.Select(e => e.Description)));
                        continue;
                    }

                    processedCount++;
                }

                _logger.LogInformation("Successfully processed {Count} standard expenses", processedCount);
                _logger.LogInformation("========== PROCESSING COMPLETE ==========");
                return processedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing standard expenses");
                return Error.Failure("StandardExpense.Processing.Failed", "Failed to process standard expenses");
            }
        }

        private async Task<ErrorOr<Success>> ProcessSingleExpenseAsync(
    StandardExpense expense,
    CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing standard expense {ExpenseId} for wallet {WalletId}",
                expense.expenseID, expense.walletID);

            var walletResult = await _walletRepository.GetWalletByIdAsync(expense.walletID, cancellationToken);
            if (walletResult.IsError)
            {
                return walletResult.Errors;
            }

            var wallet = walletResult.Value;

            var transaction = new Transaction
            {
                walletID = expense.walletID,
                categoryID = 12, 
                currencyID = wallet.currencyID,
                amount = expense.amount,
                transactionType = "Cash",
                transactionDate = DateOnly.FromDateTime(DateTime.Today),
                transactionTime = TimeOnly.FromDateTime(DateTime.Now),
                description = $"{expense.reason} - {expense.description} (Auto-generated)"
            };

            var createTransactionResult = await _transactionRepository.CreateTransactionAsync(
                transaction,
                cancellationToken);

            if (createTransactionResult.IsError)
            {
                _logger.LogError("Database error in CreateTransaction: {Error}", createTransactionResult.FirstError.Description);
                return createTransactionResult.Errors;
            }

            var nextDate = CalculateNextDate(expense.nextDate, expense.frequency);
            expense.nextDate = nextDate;

            var updateExpenseResult = await _standardExpenseRepository.UpdateStandardExpenseAsync(
                expense,
                cancellationToken);

            if (updateExpenseResult.IsError)
            {
                return updateExpenseResult.Errors;
            }

            return Result.Success;
        }

        private DateOnly CalculateNextDate(DateOnly currentDate, string frequency)
        {
            var current = currentDate.ToDateTime(TimeOnly.MinValue);
            DateTime next;

            switch (frequency.ToLower())
            {
                case "daily":
                    next = current.AddDays(1);
                    break;

                case "weekly":
                    next = current.AddDays(7);
                    break;

                case "monthly":
                    next = current.AddMonths(1);
                    break;

                case "yearly":
                    next = current.AddYears(1);
                    break;

                default:
                    _logger.LogWarning("Unknown frequency '{Frequency}', defaulting to monthly", frequency);
                    next = current.AddMonths(1);
                    break;
            }

            return DateOnly.FromDateTime(next);
        }
    }
}