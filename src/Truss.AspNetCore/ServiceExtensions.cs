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
        Action<TrussWebServiceConfiguration>? trussServiceConfiguration = null
    )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        var config = new TrussWebServiceConfiguration()
                .InstallModule<TrussBundledModule>()
                .InstallModule<TrussWebBundledModule>()
                .AddInstallationStep<ModuleServiceRegistryInstallationStep>()
                .AddInstallationStep<EndpointInstallationStep>()
                .UseConfiguration(builder.Configuration)
            as TrussWebServiceConfiguration;

        if (config is null) throw new InvalidOperationException("Unable to create TrussWebServiceConfiguration");

        trussServiceConfiguration?.Invoke(config);

        var installerServices = new ServiceCollection()
            .AddLogging(c => c.AddSerilog())
            .AddSingleton<TrussServiceConfiguration>(config)
            .AddSingleton<InstallationPipeline>()
            .AddSingleton<WebInstallationPipeline>()
            .AddSingleton<InstallationManifestGenerator>()
            .AddSingleton<InstallationManifest>(p => p
                .GetRequiredService<InstallationManifestGenerator>()
                .GenerateManifestAsync()
            );

        foreach (var installationStep in config.InstallationSteps)
        {
            installerServices.AddSingleton(
                typeof(IInstallationStep),
                installationStep
            );
        }

        _installerServices = installerServices
            .BuildServiceProvider();

        var installer = _installerServices
            .GetRequiredService<InstallationPipeline>();

        installer.Run(
            builder.Services,
            builder.Configuration
        );

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
                "Truss has not been initialized. Use the UseTruss method on WebApplicationBuilder to initialize Truss."
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