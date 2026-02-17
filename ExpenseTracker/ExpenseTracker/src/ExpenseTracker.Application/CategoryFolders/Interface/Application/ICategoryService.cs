using ExpenseTracker.Application.CategoryFolders.Data;
using ExpenseTracker.Domain.CategoryData;

namespace ExpenseTracker.Application.CategoryFolders.Interface.Application

{
    public interface ICategoryService
    {
        Task<AllCategories> GetCategoriesAsync(CancellationToken token);
        Task<Category> GetCategoryByIdAsync(int categoryId, CancellationToken token);
        Task<Category> CreateCategoryAsync(Category category, CancellationToken token);
        Task<bool> UpdateCategoryAsync(Category category, CancellationToken token);
        Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken token);
    }
}
