using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Application.AccountFolders.Services;
using ExpenseTracker.Application.CurrencyFolders.Interface.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Application.CurrencyFolders.Services

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCurrencyApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<ICurrencyService, CurrencyService>();
            return services;
        }
    }
}
