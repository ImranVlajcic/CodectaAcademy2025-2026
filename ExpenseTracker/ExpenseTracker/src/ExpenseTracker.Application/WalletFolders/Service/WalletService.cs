using ExpenseTracker.Application.WalletFolders.Data;
using ExpenseTracker.Application.WalletFolders.Interface.Application;
using ExpenseTracker.Application.WalletFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.WalletData;

namespace ExpenseTracker.Application.WalletFolders.Service
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<AllWallets> GetWalletsAsync(CancellationToken token)
        {
            var getWallets = await _walletRepository.GetWalletsAsync(token);

            return new AllWallets
            {
                wallets = getWallets
            };
        }

        public async Task<Wallet> GetWalletByIdAsync(int walletId, CancellationToken token)
        {
            var getWallet = await _walletRepository.GetWalletByIdAsync(walletId, token);

            return getWallet;
        }

        public async Task<Wallet> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            var getWallet = await _walletRepository.CreateWalletAsync(wallet, token);

            return getWallet;
        }

        public async Task<bool> UpdateWalletAsync(Wallet wallet, CancellationToken token)
        {
            var status = await _walletRepository.UpdateWalletAsync(wallet, token);

            return status;
        }

        public async Task<bool> DeleteWalletAsync(int walletId, CancellationToken token)
        {
            var status = await _walletRepository.DeleteWalletAsync(walletId, token);

            return status;
        }
    }
}
