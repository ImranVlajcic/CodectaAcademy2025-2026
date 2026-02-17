using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Application.AccountFolders;
using ExpenseTracker.Infrastructure.AccountRepos;
using ExpenseTracker.Infrastructure.AccountRepos.Options;
using ExpenseTracker.Application.AccountFolders.Services;
using ExpenseTracker.WebApi.Options;
using ExpenseTracker.Infrastructure.CategoryRepos.Options;
using ExpenseTracker.Application.CategoryFolders.Services;
using ExpenseTracker.Infrastructure.CategoryRepos;
using ExpenseTracker.Infrastructure.CurrencyRepos.Options;
using ExpenseTracker.Application.CurrencyFolders.Services;
using ExpenseTracker.Infrastructure.CurrencyRepos;
using ExpenseTracker.Infrastructure.StandardExpenseRepos.Options;
using ExpenseTracker.Application.StandardExpenseFolders.Services;
using ExpenseTracker.Infrastructure.StandardExpenseRepos;
using ExpenseTracker.Infrastructure.TransactionRepos.Options;
using ExpenseTracker.Infrastructure.TransactionRepos;
using ExpenseTracker.Application.TransactionFolders.Services;
using ExpenseTracker.Infrastructure.WalletRepos.Options;
using ExpenseTracker.Application.WalletFolders.Service;
using ExpenseTracker.Infrastructure.WalletRepos;

namespace ExpenseTracker.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddAccountOptions(configuration.GetAccountOptions());
        services.TryAddCategoryOptions(configuration.GetCategoryOptions());
        services.TryAddCurrencyOptions(configuration.GetCurrencyOptions());
        services.TryAddStandardExpenseOptions(configuration.GetStandardExpenseOptions());
        services.TryAddTransactionOptions(configuration.GetTransactionOptions());
        services.TryAddWalletOptions(configuration.GetWalletOptions());

        return services
            .AddAccountApplication()
            .AddCategoryApplication()
            .AddCurrencyApplication()
            .AddStandardExpenseApplication()
            .AddTransactionApplication()
            .AddWalletApplication();
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAccountInfrastructure(configuration).AddCategoryInfrastructure(configuration)
            .AddCurrencyInfrastructure(configuration).AddStandardExpenseInfrastructure(configuration)
            .AddTransactionInfrastructure(configuration).AddWalletInfrastructure(configuration);
    }

}