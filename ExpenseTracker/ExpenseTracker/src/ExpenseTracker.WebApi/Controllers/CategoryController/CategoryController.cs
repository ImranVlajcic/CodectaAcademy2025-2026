using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using ExpenseTracker.Contracts.CategoryContracts;
using ExpenseTracker.Domain.CategoryData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.CategoryController
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoriesAsync(cancellationToken);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(
            [FromBody] CategoryCon request,
            CancellationToken cancellationToken)
        {
            var category = new Category
            {
                categoryName = request.categoryName,
                reason = request.reason
            };

            var result = await _categoryService.CreateCategoryAsync(category, cancellationToken);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = result.categoryID },
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryCon request,
            CancellationToken cancellationToken)
        {
            var category = new Category
            {
                categoryID = id,
                categoryName = request.categoryName,
                reason = request.reason,
            };

            var result = await _categoryService.UpdateCategoryAsync(category, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
