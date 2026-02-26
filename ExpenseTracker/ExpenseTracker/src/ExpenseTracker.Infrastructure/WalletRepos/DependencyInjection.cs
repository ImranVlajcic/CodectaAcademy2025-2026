using ExpenseTracker.Application.WalletFolders.Interface.Infrastructure;
using ExpenseTracker.Infrastructure.WalletRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.WalletRepos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWalletInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddWalletOptions(configuration.GetWalletOptions());

            services.TryAddScoped<IWalletRepository, WalletRepository>();

            return services;
        }
    }
}
