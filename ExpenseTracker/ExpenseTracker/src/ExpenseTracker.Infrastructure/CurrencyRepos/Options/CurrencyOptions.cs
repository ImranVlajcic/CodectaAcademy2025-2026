using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.CurrencyRepos.Options
{
    public sealed class CurrencyOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(CurrencyOptions? options)
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

    public static class CurrencyOptionsExtensions
    {
        public static IServiceCollection TryAddCurrencyOptions(this IServiceCollection services, CurrencyOptions? options = null)
        {
            var validationResult = CurrencyOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(CurrencyOptions.SectionName, typeof(CurrencyOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static CurrencyOptions? GetCurrencyOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(CurrencyOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            CurrencyOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
