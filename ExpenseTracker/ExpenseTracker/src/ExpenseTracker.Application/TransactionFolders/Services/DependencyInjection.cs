using ExpenseTracker.Application.TransactionFolders.Interface.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Application.TransactionFolders.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTransactionApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<ITransactionService, TransactionService>();
            return services;
        }

    }
}
