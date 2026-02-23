using ErrorOr;
using ExpenseTracker.Application.CategoryFolders.Data;
using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using ExpenseTracker.Application.CategoryFolders.Interface.Infrastructure;
using ExpenseTracker.Application.TransactionFolders.Services;
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
        public async Task<ErrorOr<AllCategories>> GetCategoriesAsync(CancellationToken token)
        {
            var getCategories = await _categoryRepository.GetCategoriesAsync(token);

            if (getCategories.IsError)
            {
                return getCategories.Errors;
            }

            return new AllCategories
            {
                categories = getCategories.Value
            };
        }

        public async Task<ErrorOr<Category>> GetCategoryByIdAsync(int categoryId, CancellationToken token)
        {
            var validation = CategoryValidator.ValidateCategoryId(categoryId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId, token);

            if (getCategory.IsError)
            {
                return getCategory.Errors;
            }

            return getCategory;
        }

        public async Task<ErrorOr<Category>> CreateCategoryAsync(Category category, CancellationToken token)
        {
            var validation = CategoryValidator.ValidateForCreate(category);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getCategory = await _categoryRepository.CreateCategoryAsync(category, token);

            return getCategory;
        }

        public async Task<ErrorOr<Updated>> UpdateCategoryAsync(Category category, CancellationToken token)
        {
            var validation = CategoryValidator.ValidateForUpdate(category);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _categoryRepository.UpdateCategoryAsync(category, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }

        public async Task<ErrorOr<Deleted>> DeleteCategoryAsync(int categoryId, CancellationToken token)
        {
            var validation = CategoryValidator.ValidateCategoryId(categoryId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _categoryRepository.DeleteCategoryAsync(categoryId, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }
    }
}
