using ExpenseTracker.Application.StandardExpenseFolders.Interface.Infrastructure;
using ExpenseTracker.Infrastructure.StandardExpenseRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.StandardExpenseRepos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddStandardExpenseInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddStandardExpenseOptions(configuration.GetStandardExpenseOptions());

            services.TryAddScoped<IStandardExpenseRepository, StandardExpenseRepository>();

            return services;
        }
    }
}
