using System;
using TLGames.Infrastructure.Configuration;
using TLGames.Infrastructure.Persistence;

namespace TLGames.Application.Services
{
    static class GetProviderService
    {
        public static IServiceProvider SystemServices { get; private set; }

        public static void SetSystemServices()
        {
            SystemServices = InfrastructureServicesConfiguration.ConfigureServices();
            SnakeCaseMapperInitializer.RegisterAllEntities();
        }
    }
}
