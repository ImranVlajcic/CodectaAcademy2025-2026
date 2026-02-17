namespace ExpenseTracker.Contracts.StandardExpenseContracts

{
    public class AllStandardExpenses
    {
        public List<StandardExpenseCon> standardExpenses { get; set; }

        public AllStandardExpenses() { }
        public AllStandardExpenses(List<StandardExpenseCon> standardExpenses)
        {
            this.standardExpenses = standardExpenses;
        }
    }
}
