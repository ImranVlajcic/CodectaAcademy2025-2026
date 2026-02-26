using ExpenseTracker.Application.WalletFolders.Interface.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Application.WalletFolders.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWalletApplication(this IServiceCollection services)
        {
            services.TryAddScoped<IWalletService, WalletService>();
            return services;
        }
    }
}
