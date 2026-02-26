using ErrorOr;
using ExpenseTracker.Application.WalletFolders.Interface.Application;
using ExpenseTracker.Contracts.WalletContracts;
using ExpenseTracker.Domain.WalletData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.WebApi.Controllers.WalletController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ApiControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallets(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetWallets called by user: {UserId}", userId);
            var result = await _walletService.GetWalletsAsync(cancellationToken);

            return result.Match(
                    wallet => Ok(wallet),
                    errors => Problem(errors)
                );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetWalletById called by user: {UserId} for wallet: {WalletId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("CreateWallet called for user: {UserId}", userId);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateWallet called by user: {UserId} for wallet: {WalletId}", userId, id);
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
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteWallet called by user: {UserId} for wallet: {WalletId}", userId, id);
            var result = await _walletService.DeleteWalletAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
