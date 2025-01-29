using System.Reflection;
using Module = Truss.Modeling.Installation.Module;

namespace Truss;

public sealed class ModuleManifest
{
    public required string Name { get; init; }
    public required Module Module { get; init; }
    public required IReadOnlyCollection<Assembly> Assemblies { get; init; }
    public required IReadOnlyCollection<Type> Types { get; init; }
}