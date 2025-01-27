using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Truss;

public sealed class ModuleServiceRegistryInstallationStep : IInstallationStep
{
    private readonly ILogger<ModuleServiceRegistryInstallationStep> _logger;

    public ModuleServiceRegistryInstallationStep(
        ILogger<ModuleServiceRegistryInstallationStep> logger
    )
    {
        _logger = logger;
    }
    public void Run(
        IServiceCollection services,
        IConfiguration configuration,
        ModuleManifest moduleManifest
    )
    {
        moduleManifest.module.ConfigureServices(
            services: services,
            configuration: configuration
        );
    }
}