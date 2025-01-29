using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Truss.AspNetCore.Endpoints;

namespace Truss.AspNetCore;

public static class ServiceExtensions
{
    private static ServiceProvider? _installerServices;
    
    public static WebApplicationBuilder UseTruss(
        this WebApplicationBuilder builder,
        Action<TrussWebServiceOptions>? optionsBuilder = null
    )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        var options = new TrussWebServiceOptions()
            .InstallModule<TrussWebBundledModule>()
            .AddInstallationStep<EndpointInstallationStep>()
            .UseConfiguration(builder.Configuration);

        options.InstallerServices
            .AddLogging(c => c.AddSerilog())
            .AddSingleton<WebInstallationPipeline>();

        builder.Services.InstallTruss(
            options,
            a => optionsBuilder?.Invoke((TrussWebServiceOptions)a)
        );

        _installerServices = options.InstallationServiceProvider;

        return builder;
    }
    
    public static WebApplication UseTruss(
        this WebApplication app
    )
    {
        var logger = app.Services
            .GetRequiredService<ILogger<WebInstallationPipeline>>();
        
        if (_installerServices is null)
        {
            throw new InvalidOperationException(
                "Truss has not been initialized. Call UseTruss on the WebApplicationBuilder to initialize Truss."
            );
        }
        
        var pipeline = _installerServices
            .GetRequiredService<WebInstallationPipeline>();
        
        pipeline.Run(app);

        logger.LogDebug("Disposing Truss installation services");
        _installerServices.Dispose();
        
        logger.LogDebug("Truss installation complete");
        return app;
    }
}