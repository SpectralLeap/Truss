namespace Truss.AspNetCore;

internal sealed class InstallationManifest
{
    public IReadOnlyCollection<ModuleManifest> ModuleManifests { get; init; }
}