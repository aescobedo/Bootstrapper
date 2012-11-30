namespace Bootstrapper.Core
{
    using System;

    using Bootstrapper.Core.Abstract;

    /// <summary>
    /// A generic extension to override
    /// </summary>
    /// <typeparam name="T">The type of the di container.</typeparam>
    public class GenericExtension<T> : BootstrapperExtension<T>
    {
        /// <summary>
        /// Gets or sets the startup delegate.
        /// </summary>
        /// <value>The startup delegate.</value>
        public Action<T> StartupDelegate { get; set; }

        /// <summary>
        /// Gets or sets the before startup delegate.
        /// </summary>
        /// <value>The before startup delegate.</value>
        public Action<T> BeforeStartupDelegate { get; set; }

        /// <summary>
        /// Gets or sets the after startup delegate.
        /// </summary>
        /// <value>The after startup delegate.</value>
        public Action<T> AfterStartupDelegate { get; set; }

        /// <summary>
        /// Gets or sets the shutdown delegate.
        /// </summary>
        /// <value>The shutdown delegate.</value>
        public Action<T> ShutdownDelegate { get; set; }

        /// <summary>
        /// Called before the startup process of the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void BeforeStartup(T context)
        {
            if (this.BeforeStartupDelegate != null)
            {
                this.BeforeStartupDelegate(context);
            }
        }

        /// <summary>
        /// Called on startup.
        /// Initialize all relevant services for the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnStartup(T context)
        {
            if (this.StartupDelegate != null)
            {
                this.StartupDelegate(context);
            }
        }

        /// <summary>
        /// Called after the startup of the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void AfterStartup(T context)
        {
            if (this.AfterStartupDelegate != null)
            {
                this.AfterStartupDelegate(context);
            }
        }

        /// <summary>
        /// Called on shutdown.
        /// Release all unmanged resources.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnShutdown(T context)
        {
            if (this.ShutdownDelegate != null)
            {
                this.ShutdownDelegate(context);
            }
        }
    }
}