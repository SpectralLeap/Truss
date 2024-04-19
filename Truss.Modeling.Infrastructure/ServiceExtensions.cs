using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.Installation;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Infrastructure.Configuration;
using Truss.Modeling.Infrastructure.DefaultServices.Buses;
using Truss.Modeling.Infrastructure.DefaultServices.EventSourcingServices;
using Truss.Modeling.Infrastructure.Installation;

namespace Truss.Modeling.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussServiceConfiguration> configure
    )
    {
        return services.AddTruss(
            new EmptyConfiguration(),
            configure
        );
    }
    
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<TrussServiceConfiguration> configure
    )
    {
        var serviceConfiguration = new TrussServiceConfiguration();
        configure(serviceConfiguration);

        var moduleAssemblies = InstallModules(services, serviceConfiguration, configuration);
        var infrastructureAssemblies = InstallInfrastructure(services, serviceConfiguration, configuration);

        services.AddBusesAndDispatchers();
        if (serviceConfiguration.IsEventSourcing)
        {
            services.AddEventSourcing(
                serviceConfiguration.GetEventStoreType(),
                serviceConfiguration.GetEventStoreFactory(),
            [
                ..moduleAssemblies,
                ..infrastructureAssemblies
            ]);
        }
        
        return services;
    }
    
    private static Assembly[] InstallModules(
        IServiceCollection services,
        TrussServiceConfiguration serviceConfiguration,
        IConfiguration configuration
    )
    {
        var moduleAssemblies = serviceConfiguration.GetModuleAssemblies();
        
        var moduleInstaller = new TrussInstallerAgent(
            moduleAssemblies
        );
        
        moduleInstaller.InvokeAll<IModule>(
            installer => installer.Define(services, configuration));
        return moduleAssemblies;
    }
 
    private static Assembly[] InstallInfrastructure(
        IServiceCollection services,
        TrussServiceConfiguration serviceConfiguration,
        IConfiguration configuration
    )
    {
        var infrastructureAssemblies = serviceConfiguration.GetInfrastructureAssemblies();
        
        var infrastructureInstaller = new TrussInstallerAgent(
            infrastructureAssemblies
        );
       
        infrastructureInstaller.InvokeAll<IInfrastructure>(
            installer => installer.Define(services, serviceConfiguration, configuration));
        return infrastructureAssemblies;
    }

   private static void AddBusesAndDispatchers(
        this IServiceCollection services
    )
    {
        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDomainEventBus, DomainEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddTransient<IQueryBus, QueryBus>()
            ;
    }

    private static void AddEventSourcing(
        this IServiceCollection services,
        Type eventStoreType,
        Func<IEventStore>? eventStoreFactory,
        Assembly[] assemblies
    )
    {
        services
            .AddSingleton(new ChangeEventTypeMap(assemblies))
            .AddSingleton<ChangeEventSerializer>()
            .AddSingleton<ChangeEventDeserializer>()
            .AddTransient<IAggregateEventStreamWriter, AggregateEventStreamWriter>()
            .AddTransient<IAggregateEventStreamReader, AggregateEventStreamReader>();
        
        if (eventStoreFactory is not null)
        {
            services.AddSingleton(typeof(IEventStore), eventStoreFactory);
        }
        else
        {
            services.AddSingleton(typeof(IEventStore), eventStoreType);
        }
    }

}
