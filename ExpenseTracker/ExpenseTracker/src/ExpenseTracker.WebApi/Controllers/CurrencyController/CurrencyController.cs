using ErrorOr;
using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using ExpenseTracker.Contracts.CurrencyContracts;
using ExpenseTracker.Domain.CurrencyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.CurrencyController
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ApiControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken)
        {
            var result = await _currencyService.GetCurrenciesAsync(cancellationToken);

            return result.Match(
                    currency => Ok(currency),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var result = await _currencyService.GetCurrencyByIdAsync(id, cancellationToken);

            return result.Match(
                    currency => Ok(currency),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurrency(
            [FromBody] CurrencyCon request,
            CancellationToken cancellationToken)
        {
            var account = new Currency
            {
                currencyCode = request.currencyCode,
                currencyName = request.currencyName,
                rateToEuro = request.rateToEuro
            };

            var result = await _currencyService.CreateCurrencyAsync(account, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetCurrencyById),
                    new { id = created.currencyID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurrency(
            int id,
            [FromBody] CurrencyCon request,
            CancellationToken cancellationToken)
        {
            var account = new Currency
            {
                currencyID = id,
                currencyCode = request.currencyCode,
                currencyName = request.currencyName,
                rateToEuro = request.rateToEuro
            };

            var result = await _currencyService.UpdateCurrencyAsync(account, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id, CancellationToken cancellationToken)
        {
            var result = await _currencyService.DeleteCurrencyAsync(id, cancellationToken);

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
