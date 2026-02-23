using ErrorOr;
using ExpenseTracker.Application.WalletFolders.Interface.Application;
using ExpenseTracker.Contracts.WalletContracts;
using ExpenseTracker.Domain.WalletData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.WalletController
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ApiControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallets(CancellationToken cancellationToken)
        {
            var result = await _walletService.GetWalletsAsync(cancellationToken);

            return result.Match(
                    wallet => Ok(wallet),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
        {
            var result = await _walletService.GetWalletByIdAsync(id, cancellationToken);

            return result.Match(
                    wallet => Ok(wallet),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(
            [FromBody] WalletCon request,
            CancellationToken cancellationToken)
        {
            var wallet = new Wallet
            {
                userID = request.userID,
                currencyID = request.currencyID,
                balance = request.balance,
                purpose = request.purpose
            };

            var result = await _walletService.CreateWalletAsync(wallet, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetWalletById),
                    new { id = created.walletID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(
            int id,
            [FromBody] WalletCon request,
            CancellationToken cancellationToken)
        {
            var wallet = new Wallet
            {
                walletID = id,
                userID = request.userID,
                currencyID = request.currencyID,
                balance = request.balance,
                purpose = request.purpose
            };

            var result = await _walletService.UpdateWalletAsync(wallet, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id, CancellationToken cancellationToken)
        {
            var result = await _walletService.DeleteWalletAsync(id, cancellationToken);

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
