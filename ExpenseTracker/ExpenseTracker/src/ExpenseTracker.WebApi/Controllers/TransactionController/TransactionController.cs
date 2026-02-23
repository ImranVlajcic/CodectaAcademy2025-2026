using ErrorOr;
using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using ExpenseTracker.Contracts.TransactionContracts;
using ExpenseTracker.Domain.TransactionData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.TransactionController
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ApiControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetTransactionsAsync(cancellationToken);

            return result.Match(
                    transaction => Ok(transaction),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id, CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id, cancellationToken);

            return result.Match(
                    transaction => Ok(transaction),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] TransactionCon request,
            CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                walletID = request.walletID,
                categoryID = request.categoryID,
                currencyID = request.currencyID,
                transactionTime = request.transactionTime,
                transactionDate = request.transactionDate,
                transactionType = request.transactionType,
                description = request.description,
                amount = request.amount,
            };

            var result = await _transactionService.CreateTransactionAsync(transaction, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetTransactionById),
                    new { id = created.transactionID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(
            int id,
            [FromBody] TransactionCon request,
            CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                transactionID = id,
                walletID = request.walletID,
                categoryID = request.categoryID,
                currencyID = request.currencyID,
                transactionTime = request.transactionTime,
                transactionDate = request.transactionDate,
                transactionType = request.transactionType,
                description = request.description,
                amount = request.amount,
            };

            var result = await _transactionService.UpdateTransactionAsync(transaction, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id, CancellationToken cancellationToken)
        {
            var result = await _transactionService.DeleteTransactionAsync(id, cancellationToken);

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
