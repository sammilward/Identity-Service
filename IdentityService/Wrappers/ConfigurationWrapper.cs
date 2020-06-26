using IdentityService.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Wrappers
{
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}
