using Truss.Modeling.Installation;

namespace Truss.Infrastructure.Marten;

public static class ServiceExtensions
{
    public static TrussServiceOptions AddMartenServices(
        this TrussServiceOptions trussServiceOptions
    )
    {
        return trussServiceOptions
            .InstallModule<MartenModule>();
    }
}

public sealed class MartenModule : Module
{
    public override string Name => "Marten";
}