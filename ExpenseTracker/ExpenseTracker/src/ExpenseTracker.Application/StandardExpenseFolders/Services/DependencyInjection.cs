using ExpenseTracker.Application.StandardExpenseFolders.Interface.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Application.StandardExpenseFolders.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddStandardExpenseApplication(this IServiceCollection services)
        {
            services.TryAddScoped<IStandardExpenseService, StandardExpenseService>();
            return services;
        }
    }
}
