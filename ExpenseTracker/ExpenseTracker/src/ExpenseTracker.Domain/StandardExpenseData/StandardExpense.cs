namespace ExpenseTracker.Domain.StandardExpenseData

{
    public class StandardExpense
    {
        public int expenseID { get; set; }
        public int walletID { get; set; }
        public string reason { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public string frequency { get; set; }
        public DateOnly nextDate { get; set; }

        public StandardExpense() { }
        public StandardExpense(int expenseID, int walletID, string reason, string description, decimal amount, string frrquency, DateOnly nextDate)
        {
            this.expenseID = expenseID;
            this.walletID = walletID;
            this.reason = reason;
            this.description = description;
            this.amount = amount;
            this.frequency = frrquency;
            this.nextDate = nextDate;
        }
    }
}
