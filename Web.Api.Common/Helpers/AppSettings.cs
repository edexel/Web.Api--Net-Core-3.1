using Microsoft.Extensions.Configuration;
using System.IO;
using Web.Api.Object.ConfigureModels;

namespace Web.Api.Common.Helpers
{
    public class AppSettings
    {
        public static ObjectSetting _objectSetting;

        public static void Init()
        {
            _objectSetting = new ObjectSetting();

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfigurationRoot configurationRoot = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);
            configurationRoot = configurationBuilder.Build();

            _objectSetting.originRequest = configurationRoot.GetSection("GlobalSettings").GetSection("OriginRequest").Value;
            _objectSetting.jwtSecret = configurationRoot.GetSection("GlobalSettings").GetSection("jwtSecret").Value;
            _objectSetting.conectionStringSqlServer = configurationRoot.GetConnectionString("connectionStringSqlServer");
        }

        public static string GetConnectionStringSqlServer()
        {
            return _objectSetting.conectionStringSqlServer;
        }
        public static string GetOriginRequest()
        {
            return _objectSetting.originRequest;
        }
        public static string GetjwtSecret()
        {
            return _objectSetting.jwtSecret;
        }
    }
}


