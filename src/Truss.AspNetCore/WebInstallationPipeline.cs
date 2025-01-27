using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Truss.AspNetCore;

internal sealed class WebInstallationPipeline
{
    private readonly ILogger<WebInstallationPipeline> _logger;
    private readonly InstallationManifest _installationManifest;
    private readonly WebInstallationStep[] _webInstallationSteps;

    public WebInstallationPipeline(
        ILogger<WebInstallationPipeline> logger,
        InstallationManifest installationManifest,
        IEnumerable<IInstallationStep> installationSteps
    )
    {
        _logger = logger;
        _installationManifest = installationManifest;
        _webInstallationSteps = installationSteps
            .OfType<WebInstallationStep>()
            .ToArray();
    }
    
    public void Run(
        WebApplication app
    )
    {
        _logger.LogDebug("Running web installation pipeline steps [{StepNames}]",
            string.Join(", ", _webInstallationSteps.Select(s => s.GetType().Name))
        );
         
        foreach (var moduleManifest in _installationManifest.ModuleManifests)
        {
            foreach (var installationStep in _webInstallationSteps)
            {
                installationStep.Run(
                    app,
                    moduleManifest
                );
            }
        }
    }
}