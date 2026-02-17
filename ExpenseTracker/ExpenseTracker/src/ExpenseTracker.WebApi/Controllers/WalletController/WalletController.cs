using ExpenseTracker.Application.WalletFolders.Interface.Application;
using ExpenseTracker.Contracts.WalletContracts;
using ExpenseTracker.Domain.WalletData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.WalletController
{
    [ApiController]
    [Route("[controller]")]
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

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
        {
            var result = await _walletService.GetWalletByIdAsync(id, cancellationToken);

            return Ok(result);
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

            return CreatedAtAction(
                nameof(GetWalletById),
                new { id = result.walletID },
                result);
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id, CancellationToken cancellationToken)
        {
            var result = await _walletService.DeleteWalletAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
