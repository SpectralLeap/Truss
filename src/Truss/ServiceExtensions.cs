using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;
using Truss.ServiceInstallations;

namespace Truss;

public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussServiceConfiguration>? configure = null
    )
    {
        var serviceConfiguration = new TrussServiceConfiguration()
                .AddServiceInstallation<MediatrServiceInstallation>()
                .AddServiceInstallation<InternalServiceInstallation>();

        configure?.Invoke(serviceConfiguration);

        var installerAgent = new InstallerAgent(serviceConfiguration);

        installerAgent.RunInstallation(services);

        return services;
    }
}
