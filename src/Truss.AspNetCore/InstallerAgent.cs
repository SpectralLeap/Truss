using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Truss.AspNetCore;

internal sealed class InstallerAgent
{
    private readonly ILogger<InstallerAgent> _logger;
    private readonly InstallationPipeline _pipeline;
    private readonly InstallationManifest _manifest;

    public InstallerAgent(
        ILogger<InstallerAgent> logger,
        InstallationPipeline pipeline,
        InstallationManifest manifest
    )
    {
        _logger = logger;
        _pipeline = pipeline;
        _manifest = manifest;
    }
    public void RunInstallation(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        _logger.LogInformation("Starting Truss installation");
        
        _logger.LogDebug("Truss Installation Manifest produced");

        if (_manifest.ModuleManifests.Count == 0)
        {
            _logger.LogWarning("No modules to install. Exiting Truss installation");
            return;
        }
        
        _logger.LogDebug("Installing {ModuleCount} modules: [{Modules}]", 
            _manifest.ModuleManifests.Count,
            string.Join(", ", _manifest.ModuleManifests.Select(m => m.Name))
        );
        
        _pipeline.Run(
            services,
            configuration
        );
    }
   
}