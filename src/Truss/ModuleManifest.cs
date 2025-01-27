using System.Reflection;
using Truss.Modeling.Installation;

namespace Truss;

public sealed class ModuleManifest
{
    public required string Name { get; init; }
    public required IModule module { get; init; }
    public required IReadOnlyCollection<Assembly> Assemblies;
    public required IReadOnlyCollection<Type> Types;
}