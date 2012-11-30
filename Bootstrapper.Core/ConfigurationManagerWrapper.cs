namespace Bootstrapper.Core
{
    using System.Collections.Specialized;
    using System.Configuration;

    using Bootstrapper.Core.Abstract;

    /// <summary>
    /// Implements a configuration manager wrapper to enable TDD.
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        /// <summary>
        /// Gets the <see cref="T:System.Configuration.AppSettingsSection"/> object configuration section that applies to this <see cref="T:System.Configuration.Configuration"/> object.
        /// </summary>
        /// <value>The app settings.</value>
        /// <returns>An <see cref="T:System.Configuration.AppSettingsSection"/> object representing the appSettings configuration section that applies to this <see cref="T:System.Configuration.Configuration"/> object.</returns>
        public virtual NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Configuration.ConnectionStringsSection"/> data for the current application's default configuration.
        /// </summary>
        /// <value>The connection strings.</value>
        /// <returns>Returns a <see cref="T:System.Configuration.ConnectionStringSettingsCollection"/> object that contains the contents of the <see cref="T:System.Configuration.ConnectionStringsSection"/> object for the current application's default configuration. </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">Could not retrieve a <see cref="T:System.Configuration.ConnectionStringSettingsCollection"/> object.</exception>
        public virtual ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        /// <summary>
        /// Opens the configuration file for the current application as a <see cref="T:System.Configuration.Configuration"/> object.
        /// </summary>
        /// <param name="userLevel">The <see cref="T:System.Configuration.ConfigurationUserLevel"/> for which you are opening the configuration.</param>
        /// <returns>
        /// A <see cref="T:System.Configuration.Configuration"/> object.
        /// </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">A configuration file could not be loaded.</exception>
        public virtual IConfiguration OpenExeConfiguration(ConfigurationUserLevel userLevel)
        {
            return new ConfigurationWrapper(ConfigurationManager.OpenExeConfiguration(userLevel));
        }
    }

    /// <summary>
    /// Wrapps a configuration to enable TDD.
    /// </summary>
    public class ConfigurationWrapper : IConfiguration
    {
        /// <summary>
        /// The wrapped configuration.
        /// </summary>
        private readonly Configuration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWrapper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ConfigurationWrapper(Configuration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the connection string at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the connection string</param>
        /// <returns>The <see cref="T:System.Configuration.ConnectionStringSettings"/> object at the specified index.</returns>
        public virtual ConnectionStringSettings this[int index]
        {
            get { return this.configuration.ConnectionStrings.ConnectionStrings[index]; }
        }

        /// <summary>
        /// Gets the <see cref="System.Configuration.ConnectionStringSettings"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the conection string.</param>
        public virtual ConnectionStringSettings this[string name]
        {
            get { return this.configuration.ConnectionStrings.ConnectionStrings[name]; }
        }
    }
}