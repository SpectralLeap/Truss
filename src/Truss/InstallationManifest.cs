namespace Truss.AspNetCore;

public sealed class InstallationManifest
{
    public required IReadOnlyCollection<ModuleManifest> ModuleManifests { get; init; }
}