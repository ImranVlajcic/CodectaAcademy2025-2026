using ErrorOr;
using ExpenseTracker.Application.StandardExpenseFolders.Data;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
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
        public async Task<ErrorOr<AllStandardExpenses>> GetStandardExpensesAsync(CancellationToken token)
        {
            var getStandardExpenses = await _standardExpenseRepository.GetStandardExpensesAsync(token);

            if (getStandardExpenses.IsError)
            {
                return getStandardExpenses.Errors;
            }

            return new AllStandardExpenses
            {
                standardExpenses = getStandardExpenses.Value
            };
        }

        public async Task<ErrorOr<StandardExpense>> GetStandardExpenseByIdAsync(int expenseId, CancellationToken token)
        {
            var validation = StandardExpenseValidator.ValidateStandardExpenseId(expenseId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getStandardExpense = await _standardExpenseRepository.GetStandardExpenseByIdAsync(expenseId, token);

            if (getStandardExpense.IsError)
            {
                return getStandardExpense.Errors;
            }

            return getStandardExpense;
        }

        public async Task<ErrorOr<StandardExpense>> CreateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            var validation = StandardExpenseValidator.ValidateForCreate(standardExpense);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getStandardExpense = await _standardExpenseRepository.CreateStandardExpenseAsync(standardExpense, token);

            return getStandardExpense;
        }

        public async Task<ErrorOr<Updated>> UpdateStandardExpenseAsync(StandardExpense standardExpense, CancellationToken token)
        {
            var validation = StandardExpenseValidator.ValidateForUpdate(standardExpense);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _standardExpenseRepository.UpdateStandardExpenseAsync(standardExpense, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }

        public async Task<ErrorOr<Deleted>> DeleteStandardExpenseAsync(int expenseId, CancellationToken token)
        {
            var validation = StandardExpenseValidator.ValidateStandardExpenseId(expenseId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _standardExpenseRepository.DeleteStandardExpenseAsync(expenseId, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }
    }
}
