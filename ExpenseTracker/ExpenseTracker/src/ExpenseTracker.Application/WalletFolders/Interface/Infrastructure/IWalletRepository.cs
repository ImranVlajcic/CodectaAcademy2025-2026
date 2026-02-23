using ExpenseTracker.Domain.WalletData;
using ErrorOr;

namespace ExpenseTracker.Application.WalletFolders.Interface.Infrastructure
{
    public interface IWalletRepository
    {
        Task<ErrorOr<List<Wallet>>> GetWalletsAsync(CancellationToken token);
        Task<ErrorOr<Wallet>> GetWalletByIdAsync(int walletId, CancellationToken token);
        Task<ErrorOr<Wallet>> CreateWalletAsync(Wallet wallet, CancellationToken token);
        Task<ErrorOr<Updated>> UpdateWalletAsync(Wallet wallet, CancellationToken token);
        Task<ErrorOr<Deleted>> DeleteWalletAsync(int walletId, CancellationToken token);
    }
}
