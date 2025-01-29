using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Truss;

public sealed class InstallationPipeline
{
    private readonly ILogger<InstallationPipeline> _logger;
    private readonly InstallationManifest _installationManifest;
    private readonly IReadOnlyCollection<IInstallationStep> _steps;

    public InstallationPipeline(
        ILogger<InstallationPipeline> logger,
        InstallationManifest installationManifest,
        IEnumerable<IInstallationStep> installationSteps
    )
    {
        _logger = logger;
        _installationManifest = installationManifest;
        _steps = installationSteps.ToArray();
    }
    
    public void Run(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        _logger.LogDebug("Running installation pipeline steps [{StepNames}]",
            string.Join(", ", _steps.Select(s => s.GetType().Name))
        );
        
        foreach (var moduleManifest in _installationManifest.ModuleManifests)
        {
            foreach (var installationStep in _steps)
            {
                installationStep.Run(
                    services,
                    configuration,
                    moduleManifest
                );
            }
        }

        foreach (var serviceInstaller in _installationManifest.ServiceInstallers)
        {
            serviceInstaller.Install(
                services,
                configuration
            );
            
            serviceInstaller.Install(
                services,
                configuration,
                _installationManifest.Assemblies
            );
        }
    }
}