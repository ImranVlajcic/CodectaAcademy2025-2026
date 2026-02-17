namespace ExpenseTracker.Contracts.WalletContracts
{
    internal class AllWallets
    {
        public List<WalletCon> wallets {  get; set; }

        public AllWallets() { }

        public AllWallets(List<WalletCon> wallets)
        {   
            this.wallets = wallets;
        }
    }
}
