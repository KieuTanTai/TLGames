using Microsoft.Extensions.DependencyInjection;
using System;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;
using TLGames.Infrastructure.Data;
using TLGames.Infrastructure.Persistence;
using TLGames.Infrastructure.Persistence.Repositories;
using TLGames.Infrastructure.Services;

namespace TLGames.Infrastructure.Configuration
{
    internal static class InfrastructureServicesConfiguration
    {
        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            try
            {
                services.AddSingleton<IStringChecker, StringChecker>();
                services.AddSingleton<IStringConverter, StringConverter>();
                // try for IDbConnectionFactory
                string connectionString = AppConfigConnection.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("connection string is incorrect!");
                services.AddSingleton<IDbConnectionFactory>(provider => new MySqlConnectionFactory(connectionString));
                services.AddSingleton<IColumnService, ColumnService>();
                services.Scan(scan => scan.FromAssembliesOf(typeof(CategoryDAO))
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("DAO")))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            TLGames.Application.Services.Utils.GetProviderService.SetServiceProvider(serviceProvider);
            return serviceProvider;
        }
    }
}
