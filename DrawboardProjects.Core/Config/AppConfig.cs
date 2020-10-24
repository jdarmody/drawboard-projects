using DrawboardProjects.Core.Config.Sections;
using Microsoft.Extensions.Configuration;
using Windows.ApplicationModel;

namespace DrawboardProjects.Core.Config
{
    /// <summary>
    /// Simple read-only configuration class. Instantiate to easily retrieve the configuration.
    /// </summary>
    public class AppConfig
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Package.Current.InstalledLocation.Path)
                .AddJsonFile("appsettings.json", optional: false);

            _configurationRoot = builder.Build();
        }

        /// <summary>
        /// Unsecure storage of username and password. <b>Use only for testing purposes.</b>
        /// </summary>
        public AuthenticationForTesting AuthenticationForTesting => GetSection<AuthenticationForTesting>(nameof(AuthenticationForTesting));
        /// <summary>
        /// Configuration to be used in the API client Service classes.
        /// </summary>
        public Api Api => GetSection<Api>(nameof(Sections.Api));

        private T GetSection<T>(string key) => _configurationRoot.GetSection(key).Get<T>();
    }
}
