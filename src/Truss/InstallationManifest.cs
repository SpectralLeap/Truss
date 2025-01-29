using System.Reflection;
using Truss.Modeling.Installation;

namespace Truss;

public sealed class InstallationManifest
{
    public required IReadOnlyCollection<ModuleManifest> ModuleManifests { get; init; }
    public required IReadOnlyCollection<ServiceInstaller> ServiceInstallers { get; init; }
    public required IReadOnlyCollection<Assembly> Assemblies { get; init; }
}