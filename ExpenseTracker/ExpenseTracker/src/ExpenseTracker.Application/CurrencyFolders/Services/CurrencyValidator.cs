using ErrorOr;
using ExpenseTracker.Domain.Errors.CurrencyErrors;
using ErrorOr;
using ExpenseTracker.Domain.CurrencyData;


namespace ExpenseTracker.Application.CurrencyFolders.Services
{
    public static class CurrencyValidator
    {
        private const int MaxCodeLength = 3;
        private const int MaxNameLength = 50;

        public static ErrorOr<Success> ValidateForCreate(Currency currency)
        {
            var errors = new List<Error>();

            if (!string.IsNullOrWhiteSpace(currency.currencyCode) &&
                currency.currencyCode.Length > MaxCodeLength)
            {
                errors.Add(CurrencyErrors.Validation.CurrencyCodeTooLong);
            }

            if (!string.IsNullOrWhiteSpace(currency.currencyName) &&
                currency.currencyName.Length > MaxNameLength)
            {
                errors.Add(CurrencyErrors.Validation.CurrencyNameTooLong);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateForUpdate(Currency currency)
        {
            var errors = new List<Error>();

            if (currency.currencyID <= 0)
            {
                errors.Add(CurrencyErrors.Validation.InvalidCurrencyId);
            }

            var createValidation = ValidateForCreate(currency);
            if (createValidation.IsError)
            {
                errors.AddRange(createValidation.Errors);
            }

            return errors.Count > 0 ? errors : Result.Success;
        }

        public static ErrorOr<Success> ValidateCurrencyId(int currencyId)
        {
            if (currencyId <= 0)
            {
                return CurrencyErrors.Validation.InvalidCurrencyId;
            }

            return Result.Success;
        }
    }
}
