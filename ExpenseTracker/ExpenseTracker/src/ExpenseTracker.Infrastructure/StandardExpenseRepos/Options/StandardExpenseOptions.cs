using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.StandardExpenseRepos.Options
{
    public sealed class StandardExpenseOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(StandardExpenseOptions? options)
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

    public static class StandardExpenseOptionsExtensions
    {
        public static IServiceCollection TryAddStandardExpenseOptions(this IServiceCollection services, StandardExpenseOptions? options = null)
        {
            var validationResult = StandardExpenseOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(StandardExpenseOptions.SectionName, typeof(StandardExpenseOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static StandardExpenseOptions? GetStandardExpenseOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(StandardExpenseOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            StandardExpenseOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
