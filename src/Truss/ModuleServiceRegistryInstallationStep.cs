using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Installation;

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
        moduleManifest.Module.ConfigureServices(
            services: services,
            configuration: configuration
        );

        var serviceInstallers = moduleManifest.Types
            .Where(t => t.GetInterfaces()
                .Any(i => i == typeof(IServiceInstaller))
            )
            .ToArray();

        foreach (var serviceInstaller in serviceInstallers)
        {
            var installer = (IServiceInstaller)Activator.CreateInstance(serviceInstaller);

            installer.Install(
                services,
                configuration,
                moduleManifest.Assemblies
            );
        }
    }
}