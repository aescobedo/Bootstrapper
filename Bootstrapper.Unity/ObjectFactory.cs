namespace Bootstrapper.Unity
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements the object factory as a singleton instance.
    /// </summary>
    public class ObjectFactory
    {
        /// <summary>
        /// Provide a lock object to implement thread saved access to the singleton instance.
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// Holds a reference to the unity container.
        /// </summary>
        private static IUnityContainer container;

        /// <summary>
        /// Gets or sets the instance of the <see cref="IUnityContainer"/>.
        /// </summary>
        /// <value>The instance of the <see cref="IUnityContainer"/>.</value>
        public static IUnityContainer Container
        {
            get
            {
                lock (@lock)
                {
                    return container ?? (container = new UnityContainer());
                }
            }

            set
            {
                lock (@lock)
                {
                    container = value;
                }
            }
        }
    }
}