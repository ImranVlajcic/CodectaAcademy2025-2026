using ErrorOr;
using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using ExpenseTracker.Contracts.CategoryContracts;
using ExpenseTracker.Domain.CategoryData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.CategoryController
{
    [ApiController]
    [Route("api/[controller]")]
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

            return result.Match(
                    category => Ok(category),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            return result.Match(
                    category => Ok(category),
                    errors => Problem(errors)
                );
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
            var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        private IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(ModelStateDictionaryFrom(errors));
            }

            var firstError = errors[0];

            return Problem(
                statusCode: GetStatusCode(firstError.Type),
                title: GetTitle(firstError.Type),
                detail: firstError.Description,
                type: GetType(firstError.Type)
            );
        }

        private static int GetStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        private static string GetTitle(ErrorType errorType) => errorType switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            _ => "Internal Server Error",
        };

        private static string GetType(ErrorType errorType) => errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        };

        private static ModelStateDictionary ModelStateDictionaryFrom(List<Error> errors)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }

            return modelState;
        }
    }
}
