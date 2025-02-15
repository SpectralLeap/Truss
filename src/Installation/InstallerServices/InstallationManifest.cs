using System.Reflection;
using Installation.Abastractions.Endpoints;
using Installation.Abstractions.Services;

namespace Installation.InstallerServices;

public sealed class InstallationManifest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<Area> Areas { get;init; }
    public required IReadOnlyCollection<Module> Modules { get; init; }
    public required IReadOnlyCollection<Assembly> Assemblies { get; init; }

    public sealed class Area
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required IReadOnlyCollection<Module> Modules { get; init; }
    }

    public sealed class Module
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string PathBase { get; init; }
        public required bool UsePathBase { get; init; }
        public required IReadOnlyCollection<Assembly> Assemblies { get; init; }
        public required IReadOnlyCollection<Module> SubModules { get; init; }
        public required IReadOnlyCollection<IServicesInstaller> ServicesInstallers { get; init; }
        public required IReadOnlyCollection<IEndpointsMapper> EndpointsMappers { get; init; }
    }

}