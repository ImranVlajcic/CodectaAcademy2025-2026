using ExpenseTracker.Application.StandardExpenseFolders.Data;
using ExpenseTracker.Domain.StandardExpenseData;
using ErrorOr;

namespace ExpenseTracker.Application.StandardExpenseFolders.Interface.Application
{
    public interface IStandardExpenseService
    {
        Task<ErrorOr<AllStandardExpenses>> GetStandardExpensesAsync(CancellationToken token);
        Task<ErrorOr<StandardExpense>> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token);
        Task<ErrorOr<StandardExpense>> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteStandardExpenseAsync(int expenseId, CancellationToken token);
    }
}
