using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Installation;
using Truss.Modeling.Installation;

namespace Truss;

public sealed class InstallationManifest
{
    public required IReadOnlyCollection<ModuleManifest> ModuleManifests { get; init; }
}

public sealed class ModuleManifest
{
    public required string Name { get; init; }

    public required IReadOnlyCollection<object> Creates { get; init; }
    public required IReadOnlyCollection<object> Reads { get; init; }
    public required IReadOnlyCollection<object> Updates { get; init; }
    public required IReadOnlyCollection<object> Deletes { get; init; }
    public required IReadOnlyCollection<object> RemoteProcedureCalls { get; init; }
}

internal sealed class ModuleInstaller
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ModuleInstaller(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        _services = services;
        _configuration = configuration;
    }

    public void Install(
        IModule module
    )
    {
        module.Define(
            services: _services,
            configuration: _configuration
        );
    }
}

internal sealed class InstallerAgent
{
    private readonly TrussServiceConfiguration _trussServiceConfiguration;
    private readonly List<Assembly> _assemblies = [];
    
    public InstallerAgent(
        TrussServiceConfiguration trussServiceConfiguration
    )
    {
        _trussServiceConfiguration = trussServiceConfiguration;
    }

    public void RunInstallation(
        IServiceCollection services
    )
    {
        var configuration = _trussServiceConfiguration.Configuration;
        var modules = _trussServiceConfiguration.Modules;
        
        foreach (var module in modules)
        {
            module.Define(services, configuration);
            _assemblies.Add(module.GetType().Assembly);
        }
        
        RunInstallers(services);
    }

    private void RunInstallers(IServiceCollection services)
    {
        foreach (var installation in _trussServiceConfiguration.ServiceInstallations.ToList())
        {
            var moduleName = installation.GetType().Name;
            
            _trussServiceConfiguration.Logger?.LogInformation(
                "Installing {ModuleName}",
                moduleName
            );
           
           installation.Install(
                services,
                _trussServiceConfiguration.Configuration,
                _assemblies
            );
        }
    }
}