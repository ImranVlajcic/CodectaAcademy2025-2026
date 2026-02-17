using ExpenseTracker.Domain.CurrencyData;

namespace ExpenseTracker.Application.CurrencyFolders.Data

{
    public class AllCurrencies
    {
        public List<Currency> currencies { get; set; }

        public AllCurrencies() { }
        public AllCurrencies(List<Currency> currencies)
        {
            this.currencies = currencies;
        }
    }
}
