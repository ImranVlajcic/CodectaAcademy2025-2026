using ExpenseTracker.Domain.AccountData;
using ErrorOr;

namespace ExpenseTracker.Application.AccountFolders.Interface.Infrastructure

{
    public interface IAccountRepository
    {
        Task<ErrorOr<List<Account>>> GetAccountsAsync(CancellationToken token);
        Task<ErrorOr<Account>> GetAccountByIdAsync(int userId, CancellationToken token);
        Task<ErrorOr<Account>> CreateAccountAsync(Account account, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateAccountAsync(Account account, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteAccountAsync(int userId, CancellationToken token);

        Task<ErrorOr<Account>> GetByEmailAsync(string email, CancellationToken token);
        Task<ErrorOr<bool>> EmailExistsAsync(string email, CancellationToken token);
        Task<ErrorOr<bool>> UsernameExistsAsync(string username, CancellationToken token);
    }
}
