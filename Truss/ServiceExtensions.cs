using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Configuration;
using Truss.DefaultServices.Buses;
using Truss.DefaultServices.EventSourcingServices;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Module;

namespace Truss;

public static class ServiceExtensions
{
    public static IServiceCollection AddTruss(
        this IServiceCollection services,
        Action<TrussServiceConfiguration> configure
    )
    {
        var serviceConfiguration = new TrussServiceConfiguration();
        serviceConfiguration.UseConfiguration(new EmptyConfiguration());
        configure(serviceConfiguration);

        var configuration = serviceConfiguration.Configuration;
        var moduleAssemblies = InstallModules(services, serviceConfiguration, configuration);

        Assembly[] assemblies =
        [
            ..moduleAssemblies,
        ];
        
        services.AddBusesAndDispatchers();
        if (serviceConfiguration.IsEventSourcing)
        {
            services.AddEventSourcing(
                serviceConfiguration.GetEventStoreType(),
                serviceConfiguration.GetEventStoreFactory(),
                assemblies
            );
        }

#if NET461 || NET47 || NET48
            services.AddMediatR(assemblies);
#else
            services.AddMediatR(c =>
                c.RegisterServicesFromAssemblies(assemblies));
#endif

        
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
        
        moduleInstaller.InvokeAll<IModuleInstaller>(
            installer => installer.Install(services, configuration));
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
        Type? eventStoreType,
        Func<IServiceProvider, IEventStore>? eventStoreFactory,
        Assembly[] assemblies
    )
    {
        services
            .AddSingleton(new ChangeEventTypeMap(assemblies))
            .AddTransient<ChangeEventSerializer>()
            .AddTransient<ChangeEventDeserializer>()
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
