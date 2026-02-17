using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
using ExpenseTracker.Application.StandardExpenseFolders.Data;
using ExpenseTracker.Domain.StandardExpenseData;

namespace ExpenseTracker.Application.StandardExpenseFolders.Services
{
    public class StandardExpenseService : IStandardExpenseService
    {
        private readonly IStandardExpenseRepository _standardExpenseRepository;

        public StandardExpenseService(IStandardExpenseRepository standardExpenseRepository)
        {
            _standardExpenseRepository = standardExpenseRepository;
        }
        public async Task<AllStandardExpenses> GetStandardExpensesAsync(CancellationToken token)
        {
            var getStandardExpenses = await _standardExpenseRepository.GetStandardExpensesAsync(token);

            return new AllStandardExpenses
            {
                standardExpenses = getStandardExpenses
            };
        }

        public async Task<StandardExpense> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token)
        {
            var getStandardExpense = await _standardExpenseRepository.GetStandardExpenseByIdAsync(expenseId, token);

            return getStandardExpense;
        }

        public async Task<StandardExpense> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            var getStandardExpense = await _standardExpenseRepository.CreateStandardExpenseAsync(standardExpense, token);

            return getStandardExpense;
        }

        public async Task<bool> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            var status = await _standardExpenseRepository.UpdateStandardExpenseAsync(standardExpense, token);

            return status;
        }

        public async Task<bool> DeleteStandardExpenseAsync(int expenseId, CancellationToken token)
        {
            var status = await _standardExpenseRepository.DeleteStandardExpenseAsync(expenseId, token);

            return status;
        }
    }
}
