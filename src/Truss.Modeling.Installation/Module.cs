using System.Reflection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Base class for a module to be installed by Truss
/// </summary>
public abstract class Module
{
    /// <summary>
    /// The name of the module
    /// </summary>
    public virtual string Name => "";

    /// <summary>
    /// The assemblies to be scanned for installation
    /// </summary>
    public virtual IReadOnlyCollection<Assembly> AdditionalAssemblies => [];
    
    /// <inheritdoc />
    public sealed override bool Equals(object? obj)
    {
        return obj is Module module &&
               Name == module.Name;
    }

    /// <inheritdoc />
    public sealed override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    /// <inheritdoc />
    public sealed override string ToString()
    {
        return Name;
    }
}