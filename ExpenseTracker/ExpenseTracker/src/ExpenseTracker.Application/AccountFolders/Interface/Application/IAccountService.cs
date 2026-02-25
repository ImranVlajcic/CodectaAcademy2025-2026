using ErrorOr;
using ExpenseTracker.Application.AccountFolders.Data;
using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Interface.Application

{
    public interface IAccountService
    {
        Task<ErrorOr<AllAccounts>> GetAccountsAsync(CancellationToken token);
        Task<ErrorOr<Account>> GetAccountByIdAsync(int userId, CancellationToken token);
        Task<ErrorOr<Account>> CreateAccountAsync(Account account, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateAccountAsync(Account account, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteAccountAsync(int userId, CancellationToken token);
    }
}
