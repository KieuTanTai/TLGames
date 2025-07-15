using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Application.Services
{
    static class GetConnectionFactoryService
    {
        public static IDbConnectionFactory ConnectionFactory { get; private set; }

        public static void GetConnectionFactory()
        {
            try
            {
                ConnectionFactory = GetProviderService.SystemServices.GetRequiredService<IDbConnectionFactory>();
                using IDbConnection connection = ConnectionFactory.CreateConnection();
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when connect to db! {ex.Message}");
            }
        }
    }
}
