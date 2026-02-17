using ExpenseTracker.Contracts.CurrencyContracts;

namespace ExpenseTracker.Contracts.CurrencyContracts
{
    public class AllCurrencies
    {
        public List<CurrencyCon> currencies { get; set; }

        public AllCurrencies() { }
        public AllCurrencies(List<CurrencyCon> currencies)
        {
            this.currencies = currencies;
        }
    }
}
