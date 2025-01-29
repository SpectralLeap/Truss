using System.Reflection;

namespace Truss.Infrastructure.OpenTelemetry;

public sealed class TracingInfo
{
    public static string ServiceName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown";
    public static string Version { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0";
    public static string ActivitySourceName => $"{ServiceName}.ActivitySource";
}