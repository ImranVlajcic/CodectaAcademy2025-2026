using ErrorOr;
using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using ExpenseTracker.Contracts.TransactionContracts;
using ExpenseTracker.Domain.TransactionData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.WebApi.Controllers.TransactionController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ApiControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetTransactions called by user: {UserId}", userId);
            var result = await _transactionService.GetTransactionsAsync(cancellationToken);

            return result.Match(
                    transaction => Ok(transaction),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetTransactionById called by user: {UserId} for transaction: {TransactionId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("CreateTransaction called for user: {UserId}", userId);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateTransaction called by user: {UserId} for transaction: {TransactionId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteTransaction called by user: {UserId} for transaction: {TransactionId}", userId, id);
            var result = await _transactionService.DeleteTransactionAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
