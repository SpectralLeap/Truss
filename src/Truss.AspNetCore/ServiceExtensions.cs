using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Truss.AspNetCore.MessageToEndpointMapping;

namespace Truss.AspNetCore;

public static class ServiceExtensions
{
    private static ServiceProvider? _installerServices;
    
    public static WebApplicationBuilder UseTruss(
        this WebApplicationBuilder builder,
        Action<TrussWebServiceOptions>? optionsBuilder = null
    )
    {
        var options = new TrussWebServiceOptions()
                .InstallModule<TrussBundledModule>()
                .InstallModule<TrussWebBundledModule>()
                .AddInstallationStep<AppModuleInstallationStep>()
                .AddInstallationStep<MessageToEndpointMappingInstallationStep>()
                .UseConfiguration(builder.Configuration)
            as TrussWebServiceOptions;

        if (options is null)
        {
            throw new InvalidOperationException(
                "Failed to cast TrussWebServiceOptions"
            );
        }

        options.InstallerServices
            .AddLogging()
            .AddSingleton(options)
            .AddSingleton<TrussServiceOptions>(options)
            .AddSingleton<InstallationPipeline>()
            .AddSingleton<WebInstallationPipeline>()
            .AddSingleton<InstallationManifestGenerator>()
            .AddSingleton<InstallationManifest>(p => p
                .GetRequiredService<InstallationManifestGenerator>()
                .GenerateManifestAsync()
            );

        foreach (var installationStep in options.InstallationSteps)
        {
            options.InstallerServices.AddSingleton(
                typeof(IInstallationStep),
                installationStep
            );
        }

        optionsBuilder?.Invoke(options);

        options.BuildServiceProvider();

        _installerServices = options.InstallationServiceProvider!;

        var pipeline = _installerServices
            .GetRequiredService<WebInstallationPipeline>();

        pipeline.Run(builder);

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