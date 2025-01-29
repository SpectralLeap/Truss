using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure.OpenTelemetry;


public sealed class OpenTelemetryServiceInstaller : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOpenTelemetry()
            .ConfigureResource(builder => builder
                .AddService(
                    serviceName: TracingInfo.ServiceName,
                    serviceVersion: TracingInfo.Version,
                    serviceInstanceId: Environment.MachineName
                )
            )
            .WithTracing(builder =>
            {
                builder.SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSource(TracingInfo.ActivitySourceName);

                var tracingConfig = configuration
                    .GetSection("Tracing")
                    .Get<TracingConfig>();

                if (tracingConfig?.OtlpExporterEndpoint is not null)
                {
                    builder.AddOtlpExporter(options =>
                    {
                        options.Endpoint = tracingConfig.OtlpExporterEndpoint;
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
                }
            })
            .WithMetrics(builder =>
            {
                builder.AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http"
                    )
                    ;
            })
            ;

        services.AddSingleton(new ActivitySource(TracingInfo.ActivitySourceName));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
    }
}