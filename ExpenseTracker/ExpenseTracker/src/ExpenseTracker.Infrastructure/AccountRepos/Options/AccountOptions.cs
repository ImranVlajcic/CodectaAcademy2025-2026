using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.AccountRepos.Options
{
    public sealed class AccountOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(AccountOptions? options)
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

    public static class AccountOptionsExtensions
    {
        public static IServiceCollection TryAddAccountOptions(this IServiceCollection services, AccountOptions? options = null)
        {
            var validationResult = AccountOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(AccountOptions.SectionName, typeof(AccountOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static AccountOptions? GetAccountOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(AccountOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            AccountOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
