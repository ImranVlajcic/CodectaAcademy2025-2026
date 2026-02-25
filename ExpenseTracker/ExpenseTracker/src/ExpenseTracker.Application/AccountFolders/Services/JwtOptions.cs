using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Application.AccountFolders.Services
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; } = 60;
        public int RefreshTokenExpiryDays { get; init; } = 7;

        public static ValidateOptionsResult Validate(JwtOptions? options)
        {
            if (options is null)
            {
                return ValidateOptionsResult.Fail(
                    $"Configuration section '{SectionName}' is null.");
            }

            if (string.IsNullOrWhiteSpace(options.Secret))
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.Secret)}' is required.");
            }

            if (options.Secret.Length < 32)
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.Secret)}' must be at least 32 characters long.");
            }

            if (string.IsNullOrWhiteSpace(options.Issuer))
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.Issuer)}' is required.");
            }

            if (string.IsNullOrWhiteSpace(options.Audience))
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.Audience)}' is required.");
            }

            if (options.ExpiryMinutes <= 0)
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.ExpiryMinutes)}' must be greater than 0.");
            }

            if (options.RefreshTokenExpiryDays <= 0)
            {
                return ValidateOptionsResult.Fail(
                    $"Property '{nameof(options.RefreshTokenExpiryDays)}' must be greater than 0.");
            }

            return ValidateOptionsResult.Success;
        }
        
    }

    public static class JwtOptionsExtensions
    {
        public static IServiceCollection TryAddJwtOptions(this IServiceCollection services, JwtOptions? options = null)
        {
            var validationResult = JwtOptions.Validate(options);
            if (!validationResult.Succeeded)
            {
                throw new OptionsValidationException(JwtOptions.SectionName, typeof(JwtOptions), validationResult.Failures);
            }

            services.TryAddSingleton(options!);
            return services;
        }

        public static JwtOptions? GetJwtOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtOptions.SectionName);
            if (!section.Exists())
            {
                return null;
            }

            JwtOptions options = new();
            section.Bind(options);
            return options;
        }
    }
}
