namespace ExpenseTracker.Domain.WalletData

{
    public class Wallet
    {
        public int walletID {  get; set; }
        public int userID { get; set; }
        public  int currencyID { get; set; }
        public decimal balance { get; set; }
        public string purpose { get; set; }

        public Wallet() { }

        public Wallet(int walletID, int userID, int currencyID, decimal balance, string purpose)
        {
            this.walletID = walletID;
            this.userID = userID;
            this.currencyID = currencyID;
            this.balance = balance;
            this.purpose = purpose;
        }
    }
}
