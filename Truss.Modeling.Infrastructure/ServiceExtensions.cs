using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Infrastructure.Buses;
using Truss.Modeling.Infrastructure.EventSourcingServices;

namespace Truss.Modeling.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection InstallTruss(
        this IServiceCollection services,
        Assembly[] assemblies
    )
    {
        var types = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .ToArray()
            ;
        
        var model = new TrussDependencyModel(
            services,
            types
        );

        model.InvokeAll<ITrussServiceInstaller>(
                installer => installer.InstallServices(services));
        
        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDomainEventBus, DomainEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddTransient<IQueryBus, QueryBus>()
            .AddSingleton(new ChangeEventTypeMap(assemblies))
            .AddSingleton<ChangeEventSerializer>()
            .AddSingleton<ChangeEventDeserializer>()
            .AddTransient<IAggregateEventStreamWriter, AggregateEventStreamWriter>()
            .AddTransient<IAggregateEventStreamReader, AggregateEventStreamReader>()
            ;
                    
#if NETSTANDARD2_0
        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies([typeof(ChangeEventHandler<>).Assembly, ..assemblies]));
#endif
#if NETFRAMEWORK
        services.AddMediatR([..assemblies]);
#endif
        
        return services
           
            ;
    }
}