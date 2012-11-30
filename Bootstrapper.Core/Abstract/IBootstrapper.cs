namespace Bootstrapper.Core.Abstract
{
    /// <summary>
    /// Defines an interface of a bootstrapper.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public interface IBootstrapper<TContainer>
    {  
        /// <summary>
        /// Gets the object resolver.
        /// </summary>
        /// <value>The object resolver.</value>
        IObjectResolver ObjectResolver { get; }

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <typeparam name="T">The type of the ext.</typeparam>
        void AddExtension<T>() where T : BootstrapperExtension<TContainer>;

        /// <summary>
        /// Starts up all registered extensions.
        /// </summary>
        void Startup();

        /// <summary>
        /// Shutdowns all registered extensions.
        /// </summary>
        void Shutdown();
    }
}