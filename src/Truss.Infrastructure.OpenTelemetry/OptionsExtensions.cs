namespace Truss.Infrastructure.OpenTelemetry;

public static class OptionsExtensions
{
    public static TrussServiceOptions AddOpenTelemetry(
        this TrussServiceOptions trussServiceOptions
    )
    {
        return trussServiceOptions
            .InstallModule<OpenTelemetryModule>();
    }
}