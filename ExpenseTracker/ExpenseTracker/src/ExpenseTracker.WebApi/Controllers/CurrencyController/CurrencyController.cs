using ErrorOr;
using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using ExpenseTracker.Contracts.CurrencyContracts;
using ExpenseTracker.Domain.CurrencyData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.WebApi.Controllers.CurrencyController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurrencyController : ApiControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetCurrencies called by user: {UserId}", userId);
            var result = await _currencyService.GetCurrenciesAsync(cancellationToken);

            return result.Match(
                    currency => Ok(currency),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetCurrencyById called by user: {UserId} for currency: {CategoryId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("CreateCurrency called for user: {UserId}", userId);
            var currency = new Currency
            {
                currencyCode = request.currencyCode,
                currencyName = request.currencyName,
                rateToEuro = request.rateToEuro
            };

            var result = await _currencyService.CreateCurrencyAsync(currency, cancellationToken);

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
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateCurrency called by user: {UserId} for currency: {CategoryId}", userId, id);
            var currency = new Currency
            {
                currencyID = id,
                currencyCode = request.currencyCode,
                currencyName = request.currencyName,
                rateToEuro = request.rateToEuro
            };

            var result = await _currencyService.UpdateCurrencyAsync(currency, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteCurrency called by user: {UserId} for currency: {CurrencyId}", userId, id);
            var result = await _currencyService.DeleteCurrencyAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
