using System.Reflection;
using Truss.Modeling.Installation;

namespace Truss;

public sealed class ModuleManifest
{
    public required string Name { get; init; }
    public required IModule Module { get; init; }
    public required IReadOnlyCollection<Assembly> Assemblies { get; init; }
    public required IReadOnlyCollection<Type> Types { get; init; }
}