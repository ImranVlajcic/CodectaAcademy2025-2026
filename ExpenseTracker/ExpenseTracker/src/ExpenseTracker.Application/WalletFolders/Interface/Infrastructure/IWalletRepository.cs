using ExpenseTracker.Domain.WalletData;

namespace ExpenseTracker.Application.WalletFolders.Interface.Infrastructure
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetWalletsAsync(CancellationToken token);
        Task<Wallet?> GetWalletByIdAsync(int walletId, CancellationToken token);
        Task<Wallet> CreateWalletAsync(Wallet wallet, CancellationToken token);
        Task<bool> UpdateWalletAsync(Wallet wallet, CancellationToken token);
        Task<bool> DeleteWalletAsync(int walletId, CancellationToken token);
    }
}
