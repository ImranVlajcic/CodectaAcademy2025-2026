using ErrorOr;
using ExpenseTracker.Domain.Errors.WalletErrors;
using ExpenseTracker.Domain.WalletData;

namespace ExpenseTracker.Application.WalletFolders.Service
{
    public static class WalletValidator
    {
        private const int MaxPurposeLength = 100;

        public static ErrorOr<Success> ValidateForCreate(Wallet wallet)
        {
            var errors = new List<Error>();

            if (wallet.userID <= 0)
            {
                errors.Add(WalletErrors.Validation.InvalidUserId);
            }

            if (wallet.currencyID <= 0)
            {
                errors.Add(WalletErrors.Validation.InvalidCurrencyId);
            }

            if (!string.IsNullOrWhiteSpace(wallet.purpose) &&
                wallet.purpose.Length > MaxPurposeLength)
            {
                errors.Add(WalletErrors.Validation.PurposeTooLong);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(Wallet wallet)
        {
            var errors = new List<Error>();

            if (wallet.walletID <= 0)
            {
                errors.Add(WalletErrors.Validation.InvalidWalletId);
            }

            var createValidation = ValidateForCreate(wallet);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateTransactionId(int walletId)
        {
            if (walletId <= 0)
            {
                return WalletErrors.Validation.InvalidWalletId;
            }

            return Result.Success;
        }
    }
}
