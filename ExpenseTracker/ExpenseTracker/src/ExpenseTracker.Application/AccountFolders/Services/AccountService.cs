using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Application.AccountFolders.Data;
using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Services

{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<AllAccounts> GetAccountsAsync(CancellationToken token)
        {
            var getAccounts = await _accountRepository.GetAccountsAsync(token);

            return new AllAccounts
            {
                accounts = getAccounts
            };
        }

        public async Task<Account> GetAccountByIdAsync(int userId, CancellationToken token)
        {
            var getAccount = await _accountRepository.GetAccountByIdAsync(userId,token);

            return getAccount;
        }

        public async Task<Account> CreateAccountAsync(Account account, CancellationToken token)
        {
            var getAccount = await _accountRepository.CreateAccountAsync(account, token);

            return getAccount;
        }

        public async Task<bool> UpdateAccountAsync(Account account, CancellationToken token)
        {
            var status = await _accountRepository.UpdateAccountAsync(account, token);

            return status;
        }

        public async Task<bool> DeleteAccountAsync(int userId, CancellationToken token)
        {
            var status = await _accountRepository.DeleteAccountAsync(userId, token);

            return status;
        }
    }
}
