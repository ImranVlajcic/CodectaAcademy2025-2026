using ExpenseTracker.Application.CategoryFolders.Data;
using ExpenseTracker.Domain.CategoryData;
using ErrorOr;

namespace ExpenseTracker.Application.CategoryFolders.Interface.Application

{
    public interface ICategoryService
    {
        Task<ErrorOr<AllCategories>> GetCategoriesAsync(CancellationToken token);
        Task<ErrorOr<Category>> GetCategoryByIdAsync(int categoryId, CancellationToken token);
        Task<ErrorOr<Category>> CreateCategoryAsync(Category category, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateCategoryAsync(Category category, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteCategoryAsync(int categoryId, CancellationToken token);
    }
}
