using System;

namespace TLGames.Application.Services.Utils
{
    public static class GetProviderService
    {
#nullable enable
        private static IServiceProvider? _serviceProvider;
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public static IServiceProvider SystemServices
        {
            get
            {
                if (_serviceProvider == null)
                    throw new InvalidOperationException("IServiceProvider has not been set. Ensure SetServiceProvider is called during application startup.");
                return _serviceProvider;
            }
        }
    }
}
