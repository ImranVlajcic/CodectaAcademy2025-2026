using ExpenseTracker.Application.WalletFolders.Data;
using ExpenseTracker.Domain.WalletData;
using ErrorOr;

namespace ExpenseTracker.Application.WalletFolders.Interface.Application

{
    public interface IWalletService
    {
        Task<ErrorOr<AllWallets>> GetWalletsAsync(CancellationToken token);
        Task<ErrorOr<Wallet>> GetWalletByIdAsync(int walletId, CancellationToken token);
        Task<ErrorOr<Wallet>> CreateWalletAsync(Wallet wallet, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateWalletAsync(Wallet wallet, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteWalletAsync(int walletId, CancellationToken token);
    }
}
