using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLGames.Application.Services.Category;

namespace TLGames.Infrastructure.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<CategoryManagementService>();
            // services.AddTransient<ProductManagementService>();
            // services.AddTransient<OrderProcessingService>();

            // HOẶC, dùng Scrutor để tự động quét tất cả các service trong tầng này:
            // services.Scan(scan => scan
            //     .FromAssembliesOf(typeof(CategoryManagementService)) // Quét assembly chứa các Business Services
            //     .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service") || type.Name.EndsWith("Manager"))) // Theo quy ước đặt tên
            //     .AsSelfWithInterfaces() // Đăng ký cả chính class và tất cả các interface mà nó implement
            //     .WithTransientLifetime() // Hoặc WithScopedLifetime()
            // );

            return services;
        }
    }
}