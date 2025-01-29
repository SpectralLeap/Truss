using Microsoft.Extensions.DependencyInjection;

namespace Truss;

public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussServiceOptions>? optionsBuilder = null
    )
    {
        var options = new TrussServiceOptions();

        InstallTruss(
            services,
            options,
            optionsBuilder
        );
        
        options.BuildServiceProvider();
        
        options.InstallationServiceProvider?
            .Dispose();
        
        return services;
    }

    public static void InstallTruss(
        this IServiceCollection services,
        TrussServiceOptions options,
        Action<TrussServiceOptions>? optionsBuilder = null
    )
    {
        options.InstallModule<TrussBundledModule>();
        
        options.InstallerServices
            .AddSingleton(options)
            .AddSingleton<InstallationPipeline>()
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

        var pipeline = options.InstallationServiceProvider!
            .GetRequiredService<InstallationPipeline>();

        pipeline.Run(
            services,
            options.Configuration
        );
    }
}
