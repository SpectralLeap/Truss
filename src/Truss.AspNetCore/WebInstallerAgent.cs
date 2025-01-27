using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Truss.AspNetCore;

internal sealed class WebInstallerAgent
{
    private readonly ILogger<WebInstallerAgent> _logger;
    private readonly WebInstallationPipeline _pipeline;
    private readonly InstallationManifest _manifest;

    public WebInstallerAgent(
        ILogger<WebInstallerAgent> logger,
        WebInstallationPipeline pipeline,
        InstallationManifest manifest
    )
    {
        _logger = logger;
        _pipeline = pipeline;
        _manifest = manifest;
    }
    
    public void RunInstallation(
        WebApplication app
    )
    {
        foreach (var moduleManifest in _manifest.ModuleManifests)
        {
            _pipeline.Run(app); 
        }
    } 
}