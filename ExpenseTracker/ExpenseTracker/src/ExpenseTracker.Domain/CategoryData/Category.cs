namespace ExpenseTracker.Domain.CategoryData

{
    public class Category
    {
       public int categoryID {  get; set; }
       public string categoryName { get; set; }
       public string reason { get; set; }

       public Category() { }
       public Category(int categoryId, string categoryName, string reason)
        {
            this.categoryID = categoryId;
            this.categoryName = categoryName;
            this.reason = reason;
        }
    }
}
