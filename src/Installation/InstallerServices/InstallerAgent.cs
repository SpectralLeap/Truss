using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Installation.InstallerServices;


public sealed class ModuleInstallationReport
{
    public required string Description { get; init; }
    public required string Name { get; init; }
    public required bool ServicesInstalled { get; init; }
}

public sealed class AreaInstallationReport
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<ModuleInstallationReport> ModuleInstallationReports { get; init; }
}

internal sealed class InstallerAgent
{
    private readonly ILogger<InstallerAgent> _logger;
    private readonly InstallationManifestGenerator _installationManifestGenerator;
    private AreaInstallationReport[]? _areaInstallationReports;
    private ModuleInstallationReport[]? _moduleInstallationReports;
    private IServiceCollection? _services;
    private ConfigurationManager? _configuration;
    private InstallationManifest? _manifest;
    private readonly List<Assembly> _assemblies = new();

    public InstallerAgent(
        ILogger<InstallerAgent> logger,
        InstallationManifestGenerator installationManifestGenerator
    )
    {
        _logger = logger;
        _installationManifestGenerator = installationManifestGenerator;
    }

    public void InstallToServices(
        IServiceCollection services,
        ConfigurationManager configuration,
        Action<IReadOnlyCollection<Assembly>, IServiceCollection>[] serviceRegistrationSteps
    )
    {
        _services = services;
        _configuration = configuration;

        _manifest = _installationManifestGenerator
            .GenerateInstallationManifest();

        _services.AddSingleton(_manifest);

        _areaInstallationReports = _manifest.Areas
            .Select(InstallArea).ToArray();

        _moduleInstallationReports = _manifest.Modules
            .Select(InstallModule).ToArray();

        _assemblies.AddRange(_manifest!
            .Modules
            .SelectMany(m => m.Assemblies)
            .ToList());

        _assemblies.AddRange(_manifest.Areas
            .SelectMany(a => a.Modules)
            .SelectMany(m => m.Assemblies)
        );

        foreach (var step in serviceRegistrationSteps)
        {
            step(_assemblies, services);
        }
    }

    private AreaInstallationReport InstallArea(
        InstallationManifest.Area area
    )
    {
        _logger.LogDebug("Installing area {AreaName}", area.Name);
        var moduleInstallationReports = area.Modules
            .Select(InstallModule).ToArray();

        return new AreaInstallationReport
        {
            Name = area.Name,
            Description = area.Description,
            ModuleInstallationReports = moduleInstallationReports
        };
    }

    private ModuleInstallationReport InstallModule(
        InstallationManifest.Module module
    )
    {
        _logger.LogDebug("Installing module {ModuleName}", module.Name);

        foreach (var installer in module.ServicesInstallers)
        {
            _logger.LogDebug("Installing services from {InstallerName}", installer.GetType().Name);

            installer.InstallServices(_services!, _configuration!);
        }

        return new ModuleInstallationReport
        {
            Name = module.Name,
            Description = module.Description,
            ServicesInstalled = true
        };
    }

    public void Map(
        IEndpointRouteBuilder builder,
        Action<IEnumerable<Assembly>, IEndpointRouteBuilder>[] endpointAssemblyMappingSteps
    )
    {
        if (_manifest is null)
        {
            throw new InvalidOperationException("The installation was not initialized");
        }

        _logger.LogDebug("Mapping endpoints");

        foreach (var area in _manifest.Areas)
        {
            MapArea(builder, area);
        }

        foreach (var module in _manifest.Modules)
        {
            MapModule(builder, module);
        }

        foreach (var step in endpointAssemblyMappingSteps)
        {
            step(_assemblies, builder);
        }
    }

    private void MapArea(IEndpointRouteBuilder builder, InstallationManifest.Area area)
    {
        _logger.LogDebug("Mapping area {AreaName}", area.Name);

        foreach (var module in area.Modules)
        {
            MapModule(builder, module);
        }
    }

    private void MapModule(IEndpointRouteBuilder builder, InstallationManifest.Module module)
    {
        _logger.LogDebug("Mapping module {ModuleName}", module.Name);

        foreach (var mapper in module.EndpointsMappers)
        {
            _logger.LogDebug("Mapping endpoints from {MapperName}", mapper.GetType().Name);
            mapper.MapEndpoints(builder);
        }
    }
}
