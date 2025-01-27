using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Truss.AspNetCore;

public static class ServiceExtensions
{
    private static ServiceProvider? _installerServices;
    
    public static WebApplicationBuilder UseTruss(
        this WebApplicationBuilder builder,
        Action<TrussServiceConfiguration>? trussServiceConfiguration = null
    )
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        var config = new TrussServiceConfiguration()
            .InstallModule<TrussBundledModule>();

        config.AddInstallationStep<ModuleServiceRegistryInstallationStep>()
            .AddInstallationStep<EndpointInstallationStep>();
        
        config.UseConfiguration(builder.Configuration);

        trussServiceConfiguration?.Invoke(config);

        var installerServices = new ServiceCollection()
            .AddLogging(c => c.AddSerilog())
            .AddSingleton(config)
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
       
        var logger = _installerServices
            .GetRequiredService<ILogger<InstallationPipeline>>();
        
        logger.LogDebug("Truss installation started");
        
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