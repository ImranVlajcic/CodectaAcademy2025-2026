using ExpenseTracker.Application.WalletFolders.Data;
using ExpenseTracker.Domain.WalletData;

namespace ExpenseTracker.Application.WalletFolders.Interface.Application

{
    public interface IWalletService
    {
        Task<AllWallets> GetWalletsAsync(CancellationToken token);
        Task<Wallet> GetWalletByIdAsync(int walletId, CancellationToken token);
        Task<Wallet> CreateWalletAsync(Wallet wallet, CancellationToken token);
        Task<bool> UpdateWalletAsync(Wallet wallet, CancellationToken token);
        Task<bool> DeleteWalletAsync(int walletId, CancellationToken token);
    }
}
