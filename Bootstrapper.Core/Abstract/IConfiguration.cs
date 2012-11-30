namespace Bootstrapper.Core.Abstract
{
    using System.Configuration;

    /// <summary>
    /// Defines an interface of a configuration.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets or sets the connection string at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the connection string</param>
        /// <returns>The <see cref="T:System.Configuration.ConnectionStringSettings"/> object at the specified index.</returns>
        ConnectionStringSettings this[int index]
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="System.Configuration.ConnectionStringSettings"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the conection string.</param>
        ConnectionStringSettings this[string name]
        {
            get;
        }
    }
}