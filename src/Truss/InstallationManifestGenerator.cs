using System.Reflection;
using Truss.Modeling.Installation;

namespace Truss;

public sealed class InstallationManifestGenerator
{
    private readonly TrussServiceOptions _options;
    private readonly List<ModuleManifest> _moduleManifests = [];

    public InstallationManifestGenerator(
        TrussServiceOptions options
    )
    {
        _options = options;
    }
    
    public InstallationManifest GenerateManifestAsync()
    {
        var serviceInstallers = new List<ServiceInstaller>();
        var globalAssemblies = new List<Assembly>();
        
        foreach (var module in _options.Modules)
        {
            IReadOnlyCollection<Assembly> assemblies =
            [
                module.GetType().Assembly,
                ..module.AdditionalAssemblies
            ];
            
            var types = assemblies.SelectMany(
                assm => assm.GetTypes()
            ).ToArray();
            
            serviceInstallers.AddRange(
                types
                    .Where(t => typeof(ServiceInstaller).IsAssignableFrom(t))
                    .Select(t => Activator.CreateInstance(t) as ServiceInstaller)
                    .Where(t => t is not null)
                    .Select(t => t!)
            );
            
            globalAssemblies.AddRange(assemblies);
            
            _moduleManifests.Add(new ModuleManifest
            {
                Name = string.IsNullOrEmpty(module.Name) 
                    ? module.GetType().Name 
                    : module.Name,
                Module = module,
                Assemblies = assemblies,
                Types = types,
            });
        }

        return new InstallationManifest
        {
            ModuleManifests = _moduleManifests,
            ServiceInstallers = serviceInstallers,
            Assemblies = globalAssemblies
        };
    }
}