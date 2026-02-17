using ExpenseTracker.Application.CategoryFolders.Interface.Infrastructure;
using ExpenseTracker.Infrastructure.CategoryRepos.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Infrastructure.CategoryRepos

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCategoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddCategoryOptions(configuration.GetCategoryOptions());

            services.TryAddSingleton<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
