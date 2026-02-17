using ExpenseTracker.Application.TransactionFolders.Interface.Infrastructure;
using ExpenseTracker.Infrastructure.TransactionRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.TransactionRepos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTransactionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransactionOptions(configuration.GetTransactionOptions());

            services.TryAddSingleton<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
