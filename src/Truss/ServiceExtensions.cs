using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;
using Truss.ServiceInstallations;

namespace Truss;

public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussServiceConfiguration> configure
    )
    {
        var serviceConfiguration = new TrussServiceConfiguration()
                .AddServiceInstallation<MediatrServiceInstallation>()
                .AddServiceInstallation<BaseServiceInstallation>()
                .AddServiceInstallation<EventSourcingServiceInstallation>()
            ;

        configure(serviceConfiguration);

        var installerAgent = new InstallerAgent(serviceConfiguration);

        installerAgent.RunInstallation(services);

        return services;
    }
}
