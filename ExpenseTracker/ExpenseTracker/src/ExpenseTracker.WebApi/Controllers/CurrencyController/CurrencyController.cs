using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using ExpenseTracker.Contracts.CurrencyContracts;
using ExpenseTracker.Domain.CurrencyData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.CurrencyController
{
    [ApiController]
    [Route("[controller]")]
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

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var result = await _currencyService.GetCurrencyByIdAsync(id, cancellationToken);

            return Ok(result);
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

            return CreatedAtAction(
                nameof(GetCurrencyById),
                new { id = result.currencyID },
                result);
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id, CancellationToken cancellationToken)
        {
            var result = await _currencyService.DeleteCurrencyAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
