namespace Bootstrapper.Core.Abstract
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interface to resolve types independent of a dependency injection framework.
    /// </summary>
    public interface IObjectResolver
    {
        /// <summary>
        /// Resolves an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <returns>the resolved instance.</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves an instance of the default requested type from the container.
        /// </summary>
        /// <param name="type">The type of the instance to resolve.</param>
        /// <returns>the resolved instance.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <typeparam name="T">The type of the instances to return.</typeparam>
        /// <returns>an enumerable of the requested instances.</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <param name="type">The type of the instances to return.</param>
        /// <returns>
        /// an enumerable of the requested instances.
        /// </returns>
        IEnumerable<object> ResolveAll(Type type);
    }
}