using ErrorOr;
using ExpenseTracker.Application.AccountFolders.Data;
using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Interface.Application

{
    public interface IAccountService
    {
        Task<AllAccounts> GetAccountsAsync(CancellationToken token);
        Task<Account> GetAccountByIdAsync(int userId, CancellationToken token);
        Task<Account> CreateAccountAsync(Account account, CancellationToken token);
        Task<bool> UpdateAccountAsync(Account account, CancellationToken token);
        Task<bool> DeleteAccountAsync(int userId, CancellationToken token);
    }
}
