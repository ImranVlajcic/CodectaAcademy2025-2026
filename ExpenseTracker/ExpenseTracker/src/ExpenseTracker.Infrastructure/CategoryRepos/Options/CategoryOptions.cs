using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.CategoryRepos.Options

{
    public sealed class CategoryOptions
    {
        public const string SectionName = "Database";

        public string? ConnectionString { get; init; }

        public static ValidateOptionsResult Validate(CategoryOptions? options)
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

    public static class CategoryOptionsExtensions
    {
        public static IServiceCollection TryAddCategoryOptions(this IServiceCollection services, CategoryOptions? options = null)
        {
            var validationResult = CategoryOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(CategoryOptions.SectionName, typeof(CategoryOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static CategoryOptions? GetCategoryOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(CategoryOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            CategoryOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
