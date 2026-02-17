using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using ExpenseTracker.Contracts.StandardExpenseContracts;
using ExpenseTracker.Domain.StandardExpenseData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.StandardExpenseController
{
    [ApiController]
    [Route("[controller]")]
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

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStandardExpenseById(int id, CancellationToken cancellationToken)
        {
            var result = await _standardExpenseService.GetStandardExpenseByIdAsync(id, cancellationToken);

            return Ok(result);
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

            return CreatedAtAction(
                nameof(GetStandardExpenseById),
                new { id = result.expenseID },
                result);
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStandardExpense(int id, CancellationToken cancellationToken)
        {
            var result = await _standardExpenseService.DeleteStandardExpenseAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
