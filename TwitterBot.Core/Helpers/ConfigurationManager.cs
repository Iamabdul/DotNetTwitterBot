using System.IO;
using Microsoft.Extensions.Configuration;

namespace TwitterBot.Core.Helpers
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; }

        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile("secrets/appsettings.secrets.json", true)
                            .Build();
        }
    }
}
