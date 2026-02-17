using ExpenseTracker.Domain.TransactionData;

namespace ExpenseTracker.Application.TransactionFolders.Data
{
    public class AllTransactions
    {
        public List<Transaction> transactions { get; set; }
        public AllTransactions() { }
        public AllTransactions(List<Transaction> transactions)
        {
            this.transactions = transactions;
        }
    }
}
