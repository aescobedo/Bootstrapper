namespace Bootstrapper.Unity
{
    using System;
    using System.Collections.Generic;

    using Bootstrapper.Core.Abstract;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements a unity object resolver. 
    /// </summary>
    public class UnityObjectResolver : IObjectResolver
    {
        /// <summary>
        /// The instance of the unity container.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityObjectResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityObjectResolver(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Resolves an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <returns>the resolved instance.</returns>
        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        /// <summary>
        /// Resolves an instance of the default requested type from the container.
        /// </summary>
        /// <param name="type">The type of the instance to resolve.</param>
        /// <returns>the resolved instance.</returns>
        public object Resolve(Type type)
        {
            return this.container.Resolve(type);
        }

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <typeparam name="T">The type of the instances to return.</typeparam>
        /// <returns>an enumerable of the requested instances.</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return this.container.ResolveAll<T>();
        }

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <param name="type">The type of the instances to return.</param>
        /// <returns>
        /// an enumerable of the requested instances.
        /// </returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.container.ResolveAll(type);
        }
    }
}