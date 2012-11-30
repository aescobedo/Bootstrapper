
namespace Bootstrapper.Unity
{
    using Bootstrapper.Core;
    using Bootstrapper.Core.Abstract;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements a class to provide a startup pattern for an application.
    /// </summary>
    public sealed class UnityBootstrapper : Bootstrapper<IUnityContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityBootstrapper"/> class.
        /// </summary>
        public UnityBootstrapper()
        {
            this.objectResolver = new UnityObjectResolver(this.Context);
            this.Context.RegisterInstance(this.ObjectResolver);
            Log.Debug("Unity container created and registered.");

            this.configurationManager = new ConfigurationManagerWrapper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityBootstrapper"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public UnityBootstrapper(IConfigurationManager configurationManager) : base(configurationManager)
        {
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public override IUnityContainer Context
        {
            get { return ObjectFactory.Container; }
        }
    }
}
