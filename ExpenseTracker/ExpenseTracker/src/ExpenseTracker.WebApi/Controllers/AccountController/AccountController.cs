using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Contracts.AccountContracts;
using ExpenseTracker.Domain.AccountData;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers.AccountController
{
    [ApiController]
    [Route("[controller]")] 
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
        {
            var result = await _accountService.GetAccountsAsync(cancellationToken);

            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id, CancellationToken cancellationToken)
        {
            var result = await _accountService.GetAccountByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(
            [FromBody] AccountsCon request,
            CancellationToken cancellationToken)
        {
            var account = new Account
            {
                username = request.username,
                email = request.email,
                accPassword = request.accPassword, 
                realName = request.realName,
                realSurname = request.realSurname,
                phoneNumber = request.phoneNumber
            };

            var result = await _accountService.CreateAccountAsync(account, cancellationToken);

            return CreatedAtAction(
                nameof(GetAccountById),
                new { id = result.userID },
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(
            int id,
            [FromBody] AccountsCon request,
            CancellationToken cancellationToken)
        {
            var account = new Account
            {
                userID = id,
                username = request.username,
                email = request.email,
                accPassword = request.accPassword, 
                realName = request.realName,
                realSurname = request.realSurname,
                phoneNumber = request.phoneNumber
            };

            var result = await _accountService.UpdateAccountAsync(account, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id, CancellationToken cancellationToken)
        {
            var result = await _accountService.DeleteAccountAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
