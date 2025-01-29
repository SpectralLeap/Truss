using Microsoft.AspNetCore.Builder;
using Serilog;
using Truss.AspNetCore;

namespace Truss.Infrastructure.Serilog;

public sealed class SerilogModule : WebModule
{
    public override string Name => "Truss.Infrastructure.Serilog";

    public override void ConfigureWebApplicationBuilder(
        WebApplicationBuilder builder
    )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}