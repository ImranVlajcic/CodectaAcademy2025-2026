using ErrorOr;
using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Services;
using ExpenseTracker.Contracts.AccountContracts;
using ExpenseTracker.Domain.AccountData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.WebApi.Controllers.AccountController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IPasswordHasher passwordHasher, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetAccounts called by user: {UserId}", userId);
            var result = await _accountService.GetAccountsAsync(cancellationToken);

            return result.Match(
                    account => Ok(account),
                    errors => Problem(errors)
                );
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("GetAccountById called by user: {UserId} for account: {AccountId}", userId, id);
            var result = await _accountService.GetAccountByIdAsync(id, cancellationToken);

            return result.Match(
                    account => Ok(account),
                    errors => Problem(errors)
                );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(
            [FromBody] AccountsCon request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateAccount called for email: {Email}", request.email);
            var passwordHash = _passwordHasher.HashPassword(request.password);

            var account = new Account
            {
                username = request.username,
                email = request.email,
                passwordHash = passwordHash,  
                realName = request.realName,
                realSurname = request.realSurname,
                phoneNumber = request.phoneNumber,
                createdAt = DateTime.UtcNow,
                isActive = true
            };

            var result = await _accountService.CreateAccountAsync(account, cancellationToken);

            return result.Match(
                created => CreatedAtAction(
                    nameof(GetAccountById),
                    new { id = created.userID },
                    created),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(
            int id,
            [FromBody] AccountsCon request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("UpdateAccount called by user: {UserId} for account: {AccountId}", userId, id);
            var passwordHash = _passwordHasher.HashPassword(request.password);

            var account = new Account
            {
                userID = id,
                username = request.username,
                email = request.email,
                passwordHash = passwordHash,  
                realName = request.realName,
                realSurname = request.realSurname,
                phoneNumber = request.phoneNumber
            };

            var result = await _accountService.UpdateAccountAsync(account, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("DeleteAccount called by user: {UserId} for account: {AccountId}", userId, id);

            var result = await _accountService.DeleteAccountAsync(id, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
