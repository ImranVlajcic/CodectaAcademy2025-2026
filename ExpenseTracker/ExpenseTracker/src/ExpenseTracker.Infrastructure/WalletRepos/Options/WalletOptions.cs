using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.WalletRepos.Options
{
    public sealed class WalletOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(WalletOptions? options)
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

    public static class WalletOptionsExtensions
    {
        public static IServiceCollection TryAddWalletOptions(this IServiceCollection services, WalletOptions? options = null)
        {
            var validationResult = WalletOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(WalletOptions.SectionName, typeof(WalletOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static WalletOptions? GetWalletOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(WalletOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            WalletOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
