using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Installation.Abstractions.Services;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfig<TConfig>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null
    )
        where TConfig : class
    {
        sectionName ??= typeof(TConfig)
            .Name
            .Replace("Configuration", "")
            .Replace("Config", "")
            ;
#if DEBUG
        // TROUBLESHOOTING: place a breakpoint here to check values
        var valueObserver = configuration.GetSection(sectionName)
            .Get<TConfig>();
#endif

        services
            .Configure<TConfig>(configuration.GetSection(sectionName))
            .AddOptions<TConfig>(sectionName)
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
                // Allows using the option as a dependency
                // without the IOptions<> prefix
                .AddTransient<TConfig>(p =>
                    {
                        var options = p.GetService<IOptionsMonitor<TConfig>>()!.CurrentValue;
                        return options;
                    }
                )
            ;
    }
}