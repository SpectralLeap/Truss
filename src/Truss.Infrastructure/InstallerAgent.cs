using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure;

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

    public void RunInstallation(IServiceCollection services)
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
           
            var result = installation.Install(
                services,
                _trussServiceConfiguration.Configuration,
                _assemblies
            );

            if (result.Failed)
            {
                _trussServiceConfiguration.Logger?.LogInformation(
                    "Installation failed for {ModuleName}: {FailureMessage}",
                    moduleName,
                    result.FailureMessage
                );
            }
            
        }
    }
}