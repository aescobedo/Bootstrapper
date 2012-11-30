namespace Bootstrapper.Core.Abstract
{
    /// <summary>
    /// Defines an abstract extension for a bootstrapper module extension.
    /// Derive from this base class to create a new extension.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public abstract class BootstrapperExtension<TContainer>
    {
        /// <summary>
        /// Called before the startup process of the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void BeforeStartup(TContainer context)
        {
        }

        /// <summary>
        /// Called on startup.
        /// Initialize all relevant services for the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void OnStartup(TContainer context)
        {
        }

        /// <summary>
        /// Called after the startup of the current module.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void AfterStartup(TContainer context)
        {
        }

        /// <summary>
        /// Called on shutdown.
        /// Release all unmanged resources.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void OnShutdown(TContainer context)
        {
        }
    }
}