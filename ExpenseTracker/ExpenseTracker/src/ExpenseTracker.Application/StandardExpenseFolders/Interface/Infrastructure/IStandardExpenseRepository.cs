using ExpenseTracker.Domain.StandardExpenseData;
using ErrorOr;

namespace ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure
{
    public interface IStandardExpenseRepository
    {
        Task<ErrorOr<List<StandardExpense>>> GetStandardExpensesAsync(CancellationToken token);
        Task<ErrorOr<StandardExpense>> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token);
        Task<ErrorOr<StandardExpense>> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteStandardExpenseAsync(int expenseId, CancellationToken token);
    }
}
