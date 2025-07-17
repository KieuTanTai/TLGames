using Microsoft.Extensions.DependencyInjection;
using System;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Data;
using TLGames.Infrastructure.Persistence;
using TLGames.Infrastructure.Persistence.Repositories;
using TLGames.Infrastructure.Services;

namespace TLGames.Infrastructure.Configuration
{
    public static class InfrastructureServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringChecker, StringChecker>();
            services.AddSingleton<IStringConverter, StringConverter>();
            services.AddSingleton<IColumnService, ColumnService>();

            string connectionString = AppConfigConnection.GetConnectionString();
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string is incorrect or empty. Please check configuration.");
            services.AddSingleton<IDbConnectionFactory>(provider => new MySqlConnectionFactory(connectionString));

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ProductDAO))
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("DAO")))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.AddTransient<IDAO<CategoryModel>, CategoryDAO>();
            services.AddTransient<IGetRelativeAsync<CategoryModel>, CategoryDAO>();
            services.AddTransient<ISoftDeleteAsync<CategoryModel>, CategoryDAO>();
            services.AddTransient<IGetDataByEnum<CategoryModel>, CategoryDAO>();

            // NOTE: ProductCategoryDAO is used for managing product categories, which are linked to categories.
            services.AddTransient<IDAO<ProductCategoryModel>, ProductCategoryDAO>();
            services.AddTransient<IGetAllByIdAsync<ProductCategoryModel>, ProductCategoryDAO>();
            services.AddTransient<IGetSingleByIdsAsync<ProductCategoryModel>, ProductCategoryDAO>();
            services.AddTransient<IUpdateByOldKeyAsync<ProductCategoryModel>, ProductCategoryDAO>();
            services.AddTransient<IDeleteByIdsAsync, ProductCategoryDAO>();
            return services;
        }
    }
}
