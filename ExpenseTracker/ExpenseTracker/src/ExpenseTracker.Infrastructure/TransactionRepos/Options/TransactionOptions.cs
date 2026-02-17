using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.TransactionRepos.Options
{
    public sealed class TransactionOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(TransactionOptions? options)
        {
            if (options is null)
            {
                return ValidateOptionsResult.Fail(
                    $"Configuration section '{SectionName}' is null.");
            }

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.ConnectionString)}' is required.");
            }

            return ValidateOptionsResult.Success;
        }
    }

    public static class TransactionsOptionsExtensions
    {
        public static IServiceCollection TryAddTransactionOptions(this IServiceCollection services, TransactionOptions? options = null)
        {
            var validationResult = TransactionOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(TransactionOptions.SectionName, typeof(TransactionOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static TransactionOptions? GetTransactionOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(TransactionOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            TransactionOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
