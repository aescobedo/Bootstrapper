namespace Bootstrapper.Core.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Bootstrapper.Core.Attributes;

    using log4net;

    public class Bootstrapper<T> : IBootstrapper<T>
    {
        /// <summary>
        /// The trace logger.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Caches all extension instances.
        /// </summary>
        protected HashSet<BootstrapperExtension<T>> extensionCache;

        /// <summary>
        /// The Configuration Manager.
        /// </summary>
        protected IConfigurationManager configurationManager;

        /// <summary>
        /// The object resolver.
        /// </summary>
        protected IObjectResolver objectResolver;

        /// <summary>
        /// The disposed flag;
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper&lt;T&gt;"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.configurationManager = new ConfigurationManagerWrapper();

            Log.Debug("Initialize UnityBootstrapper.");
            this.extensionCache = new HashSet<BootstrapperExtension<T>>();
            Log.Debug("Extension cache build up.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public Bootstrapper(IConfigurationManager configurationManager) : this()
        {
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Bootstrapper&lt;T&gt;"/> class.
        /// <see cref="Bootstrapper&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        ~Bootstrapper()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public virtual T Context { get; set; }

        /// <summary>
        /// Gets the object resolver.
        /// </summary>
        /// <value>The object resolver.</value>
        public IObjectResolver ObjectResolver
        {
            get { return this.objectResolver; }
        }

        /// <summary>
        /// Starts up all registered extensions.
        /// </summary>
        public void Startup()
        {
            Log.Debug("Startup sequence started.");

            try
            {
                this.extensionCache.ToList().ForEach(e =>
                {
                    Log.DebugFormat(CultureInfo.InvariantCulture, "Calling BeforeStartup method on type {0}", e.GetType().Name);
                    e.BeforeStartup(this.Context);
                });
                this.extensionCache.ToList().ForEach(e =>
                {
                    Log.DebugFormat(CultureInfo.InvariantCulture, "Calling OnStartup method on type {0}", e.GetType().Name);
                    e.OnStartup(this.Context);
                });
                this.extensionCache.ToList().ForEach(e =>
                {
                    Log.DebugFormat(CultureInfo.InvariantCulture, "Calling AfterStartup method on type {0}", e.GetType().Name);
                    e.AfterStartup(this.Context);
                });
            }
            catch (Exception ex)
            {
                Log.Error("Startup failed.", ex);
                throw;
            }

            Log.Debug("Startup sequence finished successfully");
        }

        /// <summary>
        /// Shutdowns all registered extensions.
        /// </summary>
        public void Shutdown()
        {
            Log.Debug("Shutdown sequence started.");

            try
            {
                foreach (var extension in this.extensionCache.Reverse())
                {
                    extension.OnShutdown(this.Context);
                }
            }
            catch
            {
                Log.Error("Shutdown sequence failed.");
                throw;
            }

            Log.Debug("Bootstrapper sequence done successfully.");
        }

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        public void AddExtension<TExtension>() where TExtension : BootstrapperExtension<T>
        {
            try
            {
                var extension = Activator.CreateInstance<TExtension>();
                this.AddExtensionInstance(extension);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(CultureInfo.InvariantCulture, "Add extension type {0} failed", typeof(T).Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Adds the extension instance.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="extension">The extension.</param>
        public void AddExtensionInstance<TExtension>(TExtension extension) where TExtension : BootstrapperExtension<T>
        {
            try
            {
                Log.DebugFormat(CultureInfo.InvariantCulture, "Adding extension instance of type {0}", typeof(T).Name);
                this.InjectProperties(extension);
                this.AddExtensionToCache(extension);

                Log.DebugFormat(CultureInfo.InvariantCulture, "Extension instance of type {0} added successfull", typeof(T).Name);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(CultureInfo.InvariantCulture, "Add extension instance type {0} failed", typeof(T).Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.extensionCache.Clear();
                    this.extensionCache = null;
                    this.configurationManager = null;
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Adds the extension to cache.
        /// </summary>
        /// <param name="extension">The extension.</param>
        protected void AddExtensionToCache(BootstrapperExtension<T> extension)
        {
            Log.DebugFormat(CultureInfo.InvariantCulture, "Adding Extension of type {0}", extension.GetType().Name);
            this.extensionCache.Add(extension);
            Log.DebugFormat(CultureInfo.InvariantCulture, "Extension of type {0} added successfull", extension.GetType().Name);
        }

        /// <summary>
        /// Injects the properties.
        /// </summary>
        /// <param name="extension">The extension.</param>
        protected void InjectProperties(BootstrapperExtension<T> extension)
        {
            Log.DebugFormat(CultureInfo.InvariantCulture, "Injecting configuration properties of type {0}", extension.GetType().Name);
            InjectConfigurationProperties(extension, typeof(ConfigurationDependencyAttribute), p => this.configurationManager.AppSettings[p.Name]);
            InjectConfigurationProperties(extension, typeof(ConnectionStringDependencyAttribute), p => this.configurationManager.ConnectionStrings[p.Name].ConnectionString);
            Log.DebugFormat(CultureInfo.InvariantCulture, "Add extension instance of type {0} to cache", extension.GetType().Name);
        }

        /// <summary>
        /// Injects the configuration properties.
        /// </summary>
        /// <param name="extension">The bootstrapper extension.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="func">The func to access the configuration value.</param>
        private static void InjectConfigurationProperties(BootstrapperExtension<T> extension, Type attributeType, Func<PropertyInfo, string> func)
        {
            var query = from propertyInfo in extension.GetType().GetProperties()
                        let p = propertyInfo.GetCustomAttributes(true)
                        where p.Any(a => a.GetType().IsAssignableFrom(attributeType))
                        select propertyInfo;

            foreach (var propertyInfo in query)
            {
                try
                {
                    if (propertyInfo.GetCustomAttributes(true).Any(a => a.GetType().IsAssignableFrom(attributeType)))
                    {
                        var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                        var value = converter.ConvertFromInvariantString(func(propertyInfo));
                        Log.DebugFormat(CultureInfo.InvariantCulture, "Injecting property {0} to value {1}", propertyInfo.Name, value);
                        propertyInfo.SetValue(extension, value, null);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format(CultureInfo.InvariantCulture, "Error while inject the property {0}", propertyInfo.Name), ex);
                }
            }
        }
    }
}