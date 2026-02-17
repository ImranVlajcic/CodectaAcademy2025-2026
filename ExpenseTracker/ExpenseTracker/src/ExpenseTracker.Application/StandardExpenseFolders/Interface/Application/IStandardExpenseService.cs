using ExpenseTracker.Application.StandardExpenseFolders.Data;
using ExpenseTracker.Domain.StandardExpenseData;

namespace ExpenseTracker.Application.StandardExpenseFolders.Interface.Application
{
    public interface IStandardExpenseService
    {
        Task<AllStandardExpenses> GetStandardExpensesAsync(CancellationToken token);
        Task<StandardExpense> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token);
        Task<StandardExpense> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<bool> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token);
        Task<bool> DeleteStandardExpenseAsync(int expenseId, CancellationToken token);
    }
}
