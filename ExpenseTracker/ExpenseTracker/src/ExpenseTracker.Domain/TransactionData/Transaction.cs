using System.Transactions;

namespace ExpenseTracker.Domain.TransactionData

{
    public class Transaction
    {
        public int transactionID { get; set; }
        public int walletID { get; set; }
        public int categoryID { get; set; }
        public int currencyID { get; set; }
        public TimeOnly transactionTime { get; set; }
        public DateOnly transactionDate { get; set; }
        public string transactionType { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }

        public Transaction() { }
        public Transaction(int transactionID, int walletID, int categoryID, int currencyID, TimeOnly transactionTime, DateOnly transactionDate, string transactionType, decimal amount, string description)
        {
            this.transactionID = transactionID;
            this.walletID = walletID;
            this.categoryID = categoryID;
            this.currencyID = currencyID;
            this.transactionTime = transactionTime;
            this.transactionDate = transactionDate;
            this.transactionType = transactionType;
            this.amount = amount;
            this.description = description;
        }
    }
}
