using ErrorOr;
using ExpenseTracker.Application.TransactionFolders.Services;
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
        public async Task<ErrorOr<AllWallets>> GetWalletsAsync(CancellationToken token)
        {
            var getWallets = await _walletRepository.GetWalletsAsync(token);

            if (getWallets.IsError) {
                return getWallets.Errors;
            }

            return new AllWallets
            {
                wallets = getWallets.Value
            };
        }

        public async Task<ErrorOr<Wallet>> GetWalletByIdAsync(int walletId, CancellationToken token)
        {
            var validation = WalletValidator.ValidateTransactionId(walletId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getWallet = await _walletRepository.GetWalletByIdAsync(walletId, token);

            if (getWallet.IsError) {
                return getWallet.Errors;
            }

            return getWallet;
        }

        public async Task<ErrorOr<Wallet>> CreateWalletAsync(Wallet wallet, CancellationToken token)
        {
            var validation = WalletValidator.ValidateForCreate(wallet);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var getWallet = await _walletRepository.CreateWalletAsync(wallet, token);

            return getWallet;
        }

        public async Task<ErrorOr<Updated>> UpdateWalletAsync(Wallet wallet, CancellationToken token)
        {
            var validation = WalletValidator.ValidateForUpdate(wallet);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _walletRepository.UpdateWalletAsync(wallet, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }

        public async Task<ErrorOr<Deleted>> DeleteWalletAsync(int walletId, CancellationToken token)
        {
            var validation = WalletValidator.ValidateTransactionId(walletId);

            if (validation.IsError)
            {
                return validation.Errors;
            }

            var status = await _walletRepository.DeleteWalletAsync(walletId, token);

            if (status.IsError)
            {
                return status.Errors;
            }

            return status;
        }
    }
}
