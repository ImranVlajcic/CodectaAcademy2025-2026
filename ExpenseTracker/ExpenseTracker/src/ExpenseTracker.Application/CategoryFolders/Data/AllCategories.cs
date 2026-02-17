using ExpenseTracker.Domain.CategoryData;

namespace ExpenseTracker.Application.CategoryFolders.Data

{
    public class AllCategories
    {
        public List<Category> categories { get; set; }

        public AllCategories() { }

        public AllCategories(List<Category> categories)
        {
            this.categories = categories;
        }
    }
}
