using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using ExpenseTracker.Contracts.TransactionContracts;
using ExpenseTracker.Domain.TransactionData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.TransactionController
{
    [ApiController]
    [Route("[controller]")]
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

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id, CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id, cancellationToken);

            return Ok(result);
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

            return CreatedAtAction(
                nameof(GetTransactionById),
                new { id = result.transactionID },
                result);
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id, CancellationToken cancellationToken)
        {
            var result = await _transactionService.DeleteTransactionAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
