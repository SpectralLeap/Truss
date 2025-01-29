using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Truss.AspNetCore;

internal sealed class WebInstallationPipeline
{
    private readonly ILogger<WebInstallationPipeline> _logger;
    private readonly InstallationManifest _installationManifest;
    private readonly InstallationPipeline _installationPipeline;
    private readonly TrussWebServiceOptions _options;
    private readonly WebModuleInstallationStep[] _webInstallationSteps;

    public WebInstallationPipeline(
        ILogger<WebInstallationPipeline> logger,
        InstallationManifest installationManifest,
        InstallationPipeline installationPipeline,
        TrussWebServiceOptions options,
        IEnumerable<IInstallationStep> installationSteps
    )
    {
        _logger = logger;
        _installationManifest = installationManifest;
        _installationPipeline = installationPipeline;
        _options = options;
        _webInstallationSteps = installationSteps
            .OfType<WebModuleInstallationStep>()
            .ToArray();
    }


    public void Run(
        WebApplicationBuilder builder
    )
    {
        // Do the web module pre-install configuration of the builder
        foreach (var moduleManifest in _installationManifest.ModuleManifests)
        {
            if (moduleManifest.Module is not WebModule webModule) continue;

            webModule.ConfigureWebApplicationBuilder(
                builder
            );
        }

        // Run the normal pipeline
        _installationPipeline.Run(
            builder.Services,
            _options.Configuration
        );

    }

    public void Run(
        WebApplication app
    )
    {
        _logger.LogDebug("Running web installation pipeline steps [{StepNames}]",
            string.Join(", ", _webInstallationSteps.Select(s => s.GetType().Name))
        );

        // Run the web pipeline steps
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