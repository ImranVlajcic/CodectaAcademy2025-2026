using ExpenseTracker.Application.CategoryFolders.Data;
using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using ExpenseTracker.Application.CategoryFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.CategoryData;

namespace ExpenseTracker.Application.CategoryFolders.Services

{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository accountRepository)
        {
            _categoryRepository = accountRepository;
        }
        public async Task<AllCategories> GetCategoriesAsync(CancellationToken token)
        {
            var getCategories = await _categoryRepository.GetCategoriesAsync(token);

            return new AllCategories
            {
                categories = getCategories
            };
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId, CancellationToken token)
        {
            var getCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId, token);

            return getCategory;
        }

        public async Task<Category> CreateCategoryAsync(Category category, CancellationToken token)
        {
            var getCategory = await _categoryRepository.CreateCategoryAsync(category, token);

            return getCategory;
        }

        public async Task<bool> UpdateCategoryAsync(Category category, CancellationToken token)
        {
            var status = await _categoryRepository.UpdateCategoryAsync(category, token);

            return status;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken token)
        {
            var status = await _categoryRepository.DeleteCategoryAsync(categoryId, token);

            return status;
        }
    }
}
