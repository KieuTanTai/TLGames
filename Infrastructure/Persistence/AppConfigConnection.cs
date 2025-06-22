using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TLGames.Infrastructure.Persistence
{
    public class AppConfigConnection
    {
        private static readonly IConfigurationRoot configurationRoot;

        static AppConfigConnection()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            configurationRoot = new ConfigurationBuilder().SetBasePath(basePath)
                                    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true).Build();
        }
        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return configurationRoot.GetConnectionString(name);
        }
    }
}
