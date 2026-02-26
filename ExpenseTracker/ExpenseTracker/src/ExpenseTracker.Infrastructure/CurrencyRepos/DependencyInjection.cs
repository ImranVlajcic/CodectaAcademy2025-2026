using ExpenseTracker.Application.CurrencyFolders.Interface.Infrastructure;
using ExpenseTracker.Infrastructure.CurrencyRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.CurrencyRepos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCurrencyInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddCurrencyOptions(configuration.GetCurrencyOptions());

            services.TryAddScoped<ICurrencyRepository, CurrencyRepository>();

            return services;
        }
    }
}
