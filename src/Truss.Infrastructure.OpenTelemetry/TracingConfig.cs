namespace Truss.Infrastructure.OpenTelemetry;

public sealed class TracingConfig
{
    public Uri? OtlpExporterEndpoint { get; init; }
}