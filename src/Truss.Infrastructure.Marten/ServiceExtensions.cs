using Truss.Modeling.Installation;

namespace Truss.Infrastructure.Marten;

public static class ServiceExtensions
{
    public static TrussServiceConfiguration AddMartenServices(
        this TrussServiceConfiguration trussServiceConfiguration
    )
    {
        trussServiceConfiguration.AddServiceInstallation<MartenServiceInstallation>();

        return trussServiceConfiguration;
    }

}