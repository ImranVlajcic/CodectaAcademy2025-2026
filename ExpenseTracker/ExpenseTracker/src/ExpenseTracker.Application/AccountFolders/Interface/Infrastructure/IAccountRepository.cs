using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Interface.Infrastructure

{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccountsAsync(CancellationToken token);
        Task<Account?> GetAccountByIdAsync(int userId, CancellationToken token);
        Task<Account> CreateAccountAsync(Account account, CancellationToken token);
        Task<bool> UpdateAccountAsync(Account account, CancellationToken token);
        Task<bool> DeleteAccountAsync(int userId, CancellationToken token);
    }
}
