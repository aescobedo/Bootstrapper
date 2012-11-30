namespace Bootstrapper.Core.Attributes
{
    using System;

    /// <summary>
    /// Defines an attribute to mark a dependency to a configuration value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConnectionStringDependencyAttribute : Attribute
    {
    }
}