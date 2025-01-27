using System.Reflection;

namespace Truss.AspNetCore;

internal sealed class InstallationManifestGenerator
{
    private readonly TrussServiceConfiguration _configuration;
    private readonly List<ModuleManifest> _moduleManifests = [];

    public InstallationManifestGenerator(
        TrussServiceConfiguration configuration
    )
    {
        _configuration = configuration;
    }
    
    public InstallationManifest GenerateManifestAsync()
    {
        foreach (var module in _configuration.Modules)
        {
            Assembly[] assemblies =
            [
                module.GetType().Assembly,
                ..module.Assemblies
            ];
            
            var types = assemblies.SelectMany(
                    assm => assm.GetTypes()
                ).ToArray();
            
            _moduleManifests.Add(new ModuleManifest
            {
                Name = string.IsNullOrEmpty(module.Name) 
                    ? module.GetType().Name 
                    : module.Name,
                module = module,
                Assemblies = module.Assemblies,
                Types = types
            });
        }

        return new InstallationManifest()
        {
            ModuleManifests = _moduleManifests
        };
    }
}