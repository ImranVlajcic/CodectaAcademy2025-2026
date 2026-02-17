namespace ExpenseTracker.Contracts.AccountContracts
{
    public class AllAccounts
    {
        public List<AccountsCon> accounts { get; set; }

        public AllAccounts() { }
        public AllAccounts(List<AccountsCon> accounts)
        {
            this.accounts = accounts;
        }
    }
}
