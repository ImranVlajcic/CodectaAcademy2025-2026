using ExpenseTracker.Domain.WalletData;

namespace ExpenseTracker.Application.WalletFolders.Data
{
    public class AllWallets
    {
        public List<Wallet> wallets { get; set; }
        public AllWallets() { }
        public AllWallets(List<Wallet> wallets)
        {
            this.wallets = wallets;
        }
    }
}
