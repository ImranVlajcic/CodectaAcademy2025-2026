namespace ExpenseTracker.Domain.CurrencyData

{
    public class Currency
    {
        public int currencyID { get; set; } 
        public string currencyCode {  get; set; }
        public string currencyName { get; set; }
        public decimal rateToEuro { get; set; }

        public Currency() { }
        public Currency(int currencyID, string currencyCode, string currencyName, decimal rateToEuro)
        {
            this.currencyID = currencyID;
            this.currencyCode = currencyCode;
            this.currencyName = currencyName;
            this.rateToEuro = rateToEuro;
        }
    }
}
