using ExpenseTracker.Domain.StandardExpenseData;

namespace ExpenseTracker.Application.StandardExpenseFolders.Data
{
    public class AllStandardExpenses
    {
        public List<StandardExpense> standardExpenses { get; set; }
        public AllStandardExpenses() { }
        public AllStandardExpenses(List<StandardExpense> standardExpenses)
        {
            this.standardExpenses = standardExpenses;
        }
    }
}
