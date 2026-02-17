namespace ExpenseTracker.Contracts.TransactionContracts

{
    internal class AllTransactions
    {
        public List<TransactionCon> transactions {  get; set; }

        public AllTransactions() { }

        public AllTransactions(List<TransactionCon> transactions)
        {
            this.transactions = transactions;
        }
    }
}
