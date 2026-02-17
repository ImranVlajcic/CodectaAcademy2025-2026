using ExpenseTracker.Application.CategoryFolders.Interface.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseTracker.Application.CategoryFolders.Services

{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCategoryApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<ICategoryService, CategoryService>();
            return services;
        }
    }
}
