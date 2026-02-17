namespace ExpenseTracker.Contracts.CategoryContracts

{
    public class AllCategories
    {
        public List<CategoryCon> accounts { get; set; }

        public AllCategories() { }
        public AllCategories(List<CategoryCon> accounts)
        {
            this.accounts = accounts;
        }
    }
}
