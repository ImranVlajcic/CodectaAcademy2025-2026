using ExpenseTracker.Infrastructure.AccountRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ExpenseTracker.Application.AccountFolders.Interface.Infrastructure;

namespace ExpenseTracker.Infrastructure.AccountRepos

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddAccountOptions(configuration.GetAccountOptions());

            services.TryAddSingleton<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
