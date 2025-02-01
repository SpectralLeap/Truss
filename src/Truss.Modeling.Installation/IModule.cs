using System.Reflection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Represents a module to be installed by Truss
/// </summary>
public interface IModule
{
    /// <summary>
    /// The name of the module
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The assemblies to be scanned for installation
    /// </summary>
    IReadOnlyCollection<Assembly> AdditionalAssemblies { get; }
}