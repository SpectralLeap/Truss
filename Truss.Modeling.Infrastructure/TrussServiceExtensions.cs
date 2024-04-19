using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Infrastructure.Buses;
using Truss.Modeling.Infrastructure.EventSourcingServices;

namespace Truss.Modeling.Infrastructure;

public static class TrussServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussConfig> configBuilder
    )
    {
        var config = new TrussConfig();

        configBuilder(config);

        var moduleAssemblies = AddModules(services, config);
        var infrastructureAssemblies = AddInfrastructure(services, config);

        services.AddBusesAndDispatchers();
        if (config.IsEventSourcing)
        {
            services.AddEventSourcing(
                config.GetEventStoreType(),
                config.GetEventStoreFactory(),
            [
                ..moduleAssemblies,
                ..infrastructureAssemblies
            ]);
        }
        
        return services;
    }

    private static Assembly[] AddInfrastructure(IServiceCollection services, TrussConfig config)
    {
        var infrastructureAssemblies = config.GetInfrastructureAssemblies();
        
        var infrastructureInstaller = new TrussInstallerAgent(
            infrastructureAssemblies
        );
       
        infrastructureInstaller.InvokeAll<ITrussInfrastructureInstaller>(
            installer => installer.Install(services, config));
        return infrastructureAssemblies;
    }

    private static Assembly[] AddModules(IServiceCollection services, TrussConfig config)
    {
        var moduleAssemblies = config.GetModuleAssemblies();
        
        var moduleInstaller = new TrussInstallerAgent(
            moduleAssemblies
        );
        
        moduleInstaller.InvokeAll<ITrussModuleInstaller>(
            installer => installer.Install(services));
        return moduleAssemblies;
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
