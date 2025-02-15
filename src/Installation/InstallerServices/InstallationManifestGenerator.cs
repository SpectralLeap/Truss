using System.Reflection;
using Installation.Abastractions.Endpoints;
using Installation.Abstractions.Services;
using Installation.Configs;
using Microsoft.Extensions.Logging;

namespace Installation.InstallerServices;

internal sealed class InstallationManifestGenerator
{
    private readonly ILogger<InstallationManifestGenerator> _logger;
    private readonly ServiceInstallationConfig _config;
    private readonly Dictionary<Assembly, string> _usedAssemblies = new();

    public InstallationManifestGenerator(
        ILogger<InstallationManifestGenerator> logger,
        ServiceInstallationConfig config
    )
    {
        _logger = logger;
        _config = config;
    }

    public InstallationManifest GenerateInstallationManifest()
    {
        _logger.LogDebug("Generating installation manifest for {ServiceName}", _config.Name);

        var manifest = new InstallationManifest
        {
            Name = _config.Name,
            Description = _config.Description,
            Areas = GetAreas(_config.Areas).ToArray(),
            Modules = GetModules(_config.Modules).ToArray(),
            Assemblies = _usedAssemblies.Keys
        };

        _logger.LogDebug("Installation manifest generated");

        return manifest;
    }

    private IEnumerable<InstallationManifest.Area> GetAreas(AreaConfig[] areas)
    {
        foreach (var area in areas)
        {
            _logger.LogDebug("Generating area manifest for {AreaName}", area.Name);

            yield return new InstallationManifest.Area
            {
                Name = area.Name,
                Description = area.Description,
                Modules = GetModules(area.Modules).ToArray()
            };
        }
    }

    private IEnumerable<InstallationManifest.Module> GetModules(ModuleConfig[] modules)
    {
        foreach (var module in modules)
        {
            yield return GetModule(module);
        }
    }

    private InstallationManifest.Module GetModule(ModuleConfig module)
    {
        _logger.LogDebug("Generating module manifest for {ModuleName}", module.Name);

        List<Assembly> moduleAssemblies = [];
        List<Assembly> endpointAssemblies = [];
        List<IServicesInstaller> servicesInstallers = [];
        List<IEndpointsMapper> endpointsMappers = [];

        foreach (var moduleAssembly in module.Assemblies)
        {
            _logger.LogDebug("Generating manifest for assembly {ModuleAssembly}", moduleAssembly);

            try
            {
                var assembly = Assembly.Load(moduleAssembly);

                if (assembly is null)
                {
                    _logger.LogCritical(
                        "The module assembly {ModuleAssembly} was not found. Ensure it is referenced in the project",
                        moduleAssembly
                    );

                    throw new InvalidOperationException(
                        $"The module assembly {moduleAssembly} was not found. Ensure it is referenced in the project."
                    );
                }

                if (_usedAssemblies.TryGetValue(assembly, out var moduleName))
                {
                    _logger.LogCritical(
                        "The module assembly {ModuleAssembly} was already registered by {ModuleName}",
                        moduleAssembly,
                        moduleName
                    );

                    throw new InvalidOperationException(
                        $"The module assembly {moduleAssembly} was already registered by {moduleName}"
                    );
                }

                _usedAssemblies.Add(assembly, module.Name);

                moduleAssemblies.Add(assembly);

                servicesInstallers.AddRange(
                    assembly.GetTypes()
                        .Where(t => typeof(IServicesInstaller).IsAssignableFrom(t))
                        .Select(t => (IServicesInstaller)Activator.CreateInstance(t)!)
                );

                endpointsMappers.AddRange(
                    assembly.GetTypes()
                        .Where(t => typeof(IEndpointsMapper).IsAssignableFrom(t))
                        .Select(t => (IEndpointsMapper)Activator.CreateInstance(t)!)
                );
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    ex,
                    "The module assembly {ModuleAssembly} was not found. Ensure it is referenced in the project",
                    moduleAssembly
                );

                throw;
            }
        }

        return new InstallationManifest.Module
        {
            Name = module.Name,
            Description = module.Description,
            PathBase = module.PathBase,
            UsePathBase = module.UsePathBase,
            Assemblies = moduleAssemblies,
            ServicesInstallers = servicesInstallers,
            EndpointsMappers = endpointsMappers,
            SubModules = GetModules(module.SubModules).ToArray()
        };
    }
}