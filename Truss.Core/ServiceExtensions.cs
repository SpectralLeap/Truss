using Microsoft.Extensions.DependencyInjection;

namespace Truss.Core;

// ReSharper disable once CheckNamespace
public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussConfig> config)
    {
        var c = new TrussConfig();
        config(c);

        var actionHandlerType = typeof(IActionHandler<,>);
        var types = c.Assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == actionHandlerType))
            .ToList();

        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == actionHandlerType);
            
            services.AddTransient(interfaceType, type);
        }

        services.AddSingleton<IntegrationBus>();

        return services;
    }
}