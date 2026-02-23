using ErrorOr;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using ExpenseTracker.Contracts.StandardExpenseContracts;
using ExpenseTracker.Domain.StandardExpenseData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.StandardExpenseController
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandardExpenseController : ApiControllerBase
    {
        private readonly IStandardExpenseService _standardExpenseService;

        public StandardExpenseController(IStandardExpenseService standardExpenseService)
        {
            _standardExpenseService = standardExpenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStandardExpenses(CancellationToken cancellationToken)
        {
            var result = await _standardExpenseService.GetStandardExpensesAsync(cancellationToken);

            return result.Match(
                    standardexpense => Ok(standardexpense),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStandardExpenseById(int id, CancellationToken cancellationToken)
        {
            var result = await _standardExpenseService.GetStandardExpenseByIdAsync(id, cancellationToken);

            return result.Match(
                    standardexpense => Ok(standardexpense),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateStandardExpense(
            [FromBody] StandardExpenseCon request,
            CancellationToken cancellationToken)
        {
            var standardExpense = new StandardExpense
            {
                walletID = request.walletID,
                reason = request.reason,
                description = request.description,
                amount = request.amount,
                frequency = request.frrquency,
                nextDate = request.nextDate
            };

            var result = await _standardExpenseService.CreateStandardExpenseAsync(standardExpense, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetStandardExpenseById),
                    new { id = created.expenseID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStandardExpense(
            int id,
            [FromBody] StandardExpenseCon request,
            CancellationToken cancellationToken)
        {
            var standardExpense = new StandardExpense
            {
                expenseID = id,
                walletID = request.walletID,
                reason = request.reason,
                description = request.description,
                amount = request.amount,
                frequency = request.frrquency,
                nextDate = request.nextDate
            };

            var result = await _standardExpenseService.UpdateStandardExpenseAsync(standardExpense, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStandardExpense(int id, CancellationToken cancellationToken)
        {
            var result = await _standardExpenseService.DeleteStandardExpenseAsync(id, cancellationToken);

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
