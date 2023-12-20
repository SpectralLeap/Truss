using Microsoft.Extensions.DependencyInjection;
using Truss.Configuration;
using Truss.Drivers;
using Truss.Dsl;

namespace Truss;

// ReSharper disable once CheckNamespace
internal static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussConfig> config)
    {
        var c = new TrussConfig();
        
        config(c);
        
        var types = c.Assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .ToList();

        return services.AddSingleton<IIntegrationBus, IntegrationBus>()
                .AddAllDrivers(types)
                .AddAllDomainDsls(types)
            ;
    }
    
    private static IServiceCollection AddAllDomainDsls(
        this IServiceCollection services,
        List<Type> types)
    {
        var dslType = typeof(DomainDsl);
        
        var declarations = types
            .Where(type => dslType.IsAssignableFrom(type) && !type.IsAbstract)
            .ToList();
  
        foreach (var declaration in declarations)
        {
            services.AddSingleton(declaration);
        }
 
        return services;       
    }
    
    private static IServiceCollection AddAllDrivers(
        this IServiceCollection services,
        List<Type> types)
    {
        var driverType = typeof(Driver<>);
        var declarations = types
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == driverType))
            .ToList();
 
        foreach (var declaration in declarations)
        {
            var interfaceType = declaration.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == driverType);
             
            services.AddTransient(interfaceType, declaration);
        }

        return services;
    }
}