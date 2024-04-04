using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Truss.Application.Cqrs.Commands;
using Truss.Application.Cqrs.EventSourcing.Events;
using Truss.Application.Cqrs.EventSourcing.Reading;
using Truss.Application.Cqrs.EventSourcing.Writing;
using Truss.Application.Cqrs.Queries;
using Truss.Application.Events;
using Truss.Domain.Events;
using Truss.Domain.EventSourcing;

namespace Truss.Application;

public static class ServiceExtensions
{
    private static readonly IEnumerable<TypeInfo> 
        ChangeEventTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IChangeEvent)))
        ;
    
    public static IServiceCollection AddDomainEvents(
        this IServiceCollection services
    )
    {
        return services
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IEventBus, EventBus>()
            ;
    }

    public static IServiceCollection AddCqrs(
        this IServiceCollection services
    )
    {
        return services
                .AddDomainEvents()
                .AddTransient<ICommandBus, CommandBus>()
                .AddTransient<IQueryBus, QueryBus>()
            ;
    }

    public static IServiceCollection AddEventSourcing(
        this IServiceCollection services
    )
    {
        var mapper = new ChangeEventTypeMap();
        
        foreach (var type in ChangeEventTypes)
        {
            mapper.Add(type);
        }
        
        services
            .AddSingleton(mapper)
            .AddSingleton<ChangeEventSerializer>()
            .AddSingleton<ChangeEventDeserializer>()
            ;
        
        services.TryAddTransient<
            IAggregateEventStreamWriter,
            AggregateEventStreamWriter>();
        services.TryAddTransient<IAggregateEventStreamReader,
            AggregateEventStreamReader>();

        return services;
    }
}