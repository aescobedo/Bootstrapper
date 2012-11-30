namespace Bootstrapper.Core.Abstract
{
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// Defines an interface of a configuration manager.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Gets the <see cref="T:System.Configuration.AppSettingsSection"/> object configuration section that applies to this <see cref="T:System.Configuration.Configuration"/> object.
        /// </summary>
        /// <value>The app settings.</value>
        /// <returns>An <see cref="T:System.Configuration.AppSettingsSection"/> object representing the appSettings configuration section that applies to this <see cref="T:System.Configuration.Configuration"/> object.</returns>
        NameValueCollection AppSettings { get; }

        /// <summary>
        ///  Gets the <see cref="T:System.Configuration.ConnectionStringsSection"/> data for the current application's default configuration.
        /// </summary>
        /// <value>The connection strings.</value>
        /// <returns>Returns a <see cref="T:System.Configuration.ConnectionStringSettingsCollection"/> object that contains the contents of the <see cref="T:System.Configuration.ConnectionStringsSection"/> object for the current application's default configuration. </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">Could not retrieve a <see cref="T:System.Configuration.ConnectionStringSettingsCollection"/> object.</exception>
        ConnectionStringSettingsCollection ConnectionStrings { get; }

        /// <summary>
        /// Opens the configuration file for the current application as a <see cref="T:System.Configuration.Configuration"/> object.
        /// </summary>
        /// <param name="userLevel">The <see cref="T:System.Configuration.ConfigurationUserLevel"/> for which you are opening the configuration.</param>
        /// <returns>
        /// A <see cref="T:System.Configuration.Configuration"/> object.
        /// </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">A configuration file could not be loaded.</exception>
        IConfiguration OpenExeConfiguration(ConfigurationUserLevel userLevel);
    }
}