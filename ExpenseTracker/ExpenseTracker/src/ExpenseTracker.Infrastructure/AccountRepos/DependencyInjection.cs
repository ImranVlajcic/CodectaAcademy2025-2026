using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;
using ExpenseTracker.Application.AccountFolders.Services;
using ExpenseTracker.Infrastructure.AccountRepos.Options;
using ExpenseTracker.Infrastructure.Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.AccountRepos

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddAccountOptions(configuration.GetAccountOptions());

            services.TryAddJwtOptions(configuration.GetJwtOptions());

            services.TryAddScoped<IAccountRepository, AccountRepository>();

            services.TryAddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.TryAddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
