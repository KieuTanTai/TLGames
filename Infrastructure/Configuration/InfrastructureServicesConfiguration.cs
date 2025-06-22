using Microsoft.Extensions.DependencyInjection;
using System;
using TLGames.Core.Interfaces;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return services.BuildServiceProvider();
        }
    }
}
