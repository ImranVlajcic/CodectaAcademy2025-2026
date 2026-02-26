using ErrorOr;
using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using ExpenseTracker.Contracts.CategoryContracts;
using ExpenseTracker.Domain.CategoryData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.CategoryController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetCategories called by user: {UserId}", userId);
            var result = await _categoryService.GetCategoriesAsync(cancellationToken);

            return result.Match(
                    category => Ok(category),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetCategoryById called by user: {UserId} for category: {CategoryId}", userId, id);
            var result = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            return result.Match(
                    category => Ok(category),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CategoryCon request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("CreateCategory called for user: {UserId}", userId);
            var category = new Category
            {
                categoryName = request.categoryName,
                reason = request.reason
            };

            var result = await _categoryService.CreateCategoryAsync(category, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetCategoryById),
                    new { id = created.categoryID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryCon request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateCategory called by user: {UserId} for category: {CategoryId}", userId, id);
            var category = new Category
            {
                categoryID = id,
                categoryName = request.categoryName,
                reason = request.reason,
            };

            var result = await _categoryService.UpdateCategoryAsync(category, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteCategory called by user: {UserId} for currency: {CategoryId}", userId, id);
            var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
