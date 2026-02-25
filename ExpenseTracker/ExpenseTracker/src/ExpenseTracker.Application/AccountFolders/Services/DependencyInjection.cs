using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ExpenseTracker.Application.AccountFolders.Interface.Application;

namespace ExpenseTracker.Application.AccountFolders.Services
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddAccountApplication(this IServiceCollection services)
        {
            services.TryAddScoped<IAccountService, AccountService>();

            services.TryAddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
