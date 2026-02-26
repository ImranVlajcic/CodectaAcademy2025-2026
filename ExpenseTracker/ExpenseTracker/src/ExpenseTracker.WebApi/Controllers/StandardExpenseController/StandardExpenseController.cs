using ErrorOr;
using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using ExpenseTracker.Contracts.StandardExpenseContracts;
using ExpenseTracker.Domain.StandardExpenseData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.WebApi.Controllers.StandardExpenseController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StandardExpenseController : ApiControllerBase
    {
        private readonly IStandardExpenseService _standardExpenseService;
        private readonly ILogger<StandardExpenseController> _logger;

        public StandardExpenseController(IStandardExpenseService standardExpenseService, ILogger<StandardExpenseController> logger)
        {
            _standardExpenseService = standardExpenseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStandardExpenses(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetStandardExpenses called by user: {UserId}", userId);
            var result = await _standardExpenseService.GetStandardExpensesAsync(cancellationToken);

            return result.Match(
                    standardexpense => Ok(standardexpense),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStandardExpenseById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetStandardById called by user: {UserId} for standard expense: {ExpenseId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("CreateStandardExpense called for user: {UserId}", userId);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateStandardExpense called by user: {UserId} for standard expense: {ExpenseId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteStandardExpense called by user: {UserId} for standard expense: {ExpenseId}", userId, id);
            var result = await _standardExpenseService.DeleteStandardExpenseAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
