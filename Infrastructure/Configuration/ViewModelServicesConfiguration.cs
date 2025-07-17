using Microsoft.Extensions.DependencyInjection;
using TLGames.Application.Services.Category;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IServices.ICategory;
using TLGames.WPFUI.ViewModels;

namespace TLGames.Infrastructure.Configuration
{
    public static class ViewModelServicesConfiguration
    {
        public static IServiceCollection AddViewModelServices(this IServiceCollection services)
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ICategoryManagementService<CategoryModel>, CategoryManagementService>();
            return services;
        }
    }
}
