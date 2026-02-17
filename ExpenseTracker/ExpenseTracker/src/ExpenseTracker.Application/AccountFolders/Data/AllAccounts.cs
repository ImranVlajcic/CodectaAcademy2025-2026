using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Data

{
    public class AllAccounts
    {
        public List<Account> accounts { get; set; }

        public AllAccounts() { }
        public AllAccounts(List<Account> accounts)
        {
            this.accounts = accounts;
        }
    }
}
