using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.MediatR;


public static class ServiceRegistrar
{
    private static readonly IEnumerable<TypeInfo> 
        ChangeEventTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IChangeEvent)))
        ;

    public static IServiceCollection AddTrussWithMediatR(
        this IServiceCollection services,
        Assembly[] assemblies
    )
    {
        return services.AddMediatR(c => 
                    c.RegisterServicesFromAssemblies([..assemblies]))
                .AddDynamicMediatRHandlers([..assemblies])
                .AddDomainEventServices()
                .AddCqrsServices()
                .AddEventSourcingServices()
            ;
    }
    
    private static IServiceCollection AddDomainEventServices(
        this IServiceCollection services
    )
    {
        return services
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IDomainEventBus, DomainEventBus>()
                .AddTransient<IChangeEventBus, ChangeEventBus>()
            ;
    }

    private static IServiceCollection AddCqrsServices(
        this IServiceCollection services
    )
    {
        return services
                .AddDomainEventServices()
                .AddTransient<ICommandBus, CommandBus>()
                .AddTransient<IQueryBus, QueryBus>()
            ;
    }

    private static IServiceCollection AddEventSourcingServices(
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
    
    private static IServiceCollection AddDynamicMediatRHandlers(
        this IServiceCollection services,
        params Assembly[] assembliesWithHandlers
    )
    {
        return services
                .AddDomainEventHandlers(assembliesWithHandlers)
                .AddSingleArityCommandHandlers(assembliesWithHandlers)
                .AddDoubleArityHandlers(assembliesWithHandlers)
                .AddQueryHandlers(assembliesWithHandlers)
                .AddChangeEventHandlers([typeof(ChangeEventHandler).Assembly, ..assembliesWithHandlers])
            ;
    }

    private static IServiceCollection AddChangeEventHandlers(
        this IServiceCollection services,
        Assembly[] assembliesWithHandlers
    )
    {
        var changeEventHandlerTypes = assembliesWithHandlers
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(IChangeEventHandler<>)
                            )
                )
            ;
     
        services.TryAddTransient<IChangeEventHandler<ChangeEvent>, ChangeEventHandler>();
        services.TryAddTransient<INotificationHandler<MediatRChangeEventWrapper<ChangeEvent>>,
            MediatRWrappedChangeEventHandlerAdapter<ChangeEvent>>();

        return services;
    }
    
    private static IServiceCollection AddDomainEventHandlers(
        this IServiceCollection services,
        Assembly[] assembliesWithHandlers
    )
    {
        var domainEventHandlerTypes = assembliesWithHandlers
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                            )
                )
            ;
         
        foreach (var domainEventHandlerType in domainEventHandlerTypes)
        {
            var domainEventType = domainEventHandlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                .GetGenericArguments()[0];
                     
            var wrapperType = typeof(MediatRDomainEventWrapper<>).MakeGenericType(domainEventType);
            var wrappedHandlerType = typeof(MediatRWrappedDomainEventHandlerAdapter<>).MakeGenericType(domainEventType);
            var genType = typeof(IDomainEventHandler<>).MakeGenericType(domainEventType);
            var wrappedGenType = typeof(INotificationHandler<>).MakeGenericType(wrapperType);
         
            services.TryAddTransient(genType, domainEventHandlerType);
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }
    
        return services;
    }
     
    private static IServiceCollection AddSingleArityCommandHandlers(
        this IServiceCollection services,
        Assembly[] assembliesWithHandlers
    )
    {
        var handlerTypes = assembliesWithHandlers
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                            )
                )
            ;

        foreach (var handlerType in handlerTypes)
        {
            var commandType = handlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .GetGenericArguments()[0];
            
            var wrapperType = typeof(MediatRCommandWrapper<>).MakeGenericType(commandType);
            var wrappedHandlerType = typeof(MediatRWrappedCommandHandlerAdapter<>).MakeGenericType(commandType);
            var genType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<Nil>));

            services.TryAddTransient(genType, handlerType);
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }

        return services;
    }

    private static IServiceCollection AddDoubleArityHandlers(
        this IServiceCollection services,
        Assembly[] assembliesWithHandlers
    )
    {
        var commandHandlerTypes = assembliesWithHandlers
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                            )
                )
            ;
     
        foreach (var commandHandlerType in commandHandlerTypes)
        {
            var commandType = commandHandlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                .GetGenericArguments()[0];
                 
            var resultType = commandHandlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                .GetGenericArguments()[1];
     
            var wrapperType = typeof(MediatRCommandWrapper<,>).MakeGenericType(commandType, resultType);
            var wrappedHandlerType = typeof(MediatRWrappedCommandHandlerAdapter<,>).MakeGenericType(commandType, resultType);
            var genType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<>).MakeGenericType(resultType));
     
            services.TryAddTransient(genType, commandHandlerType);
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }

        return services;
    }
    
    private static IServiceCollection AddQueryHandlers(
        this IServiceCollection services,
        Assembly[] assembliesWithHandlers
    )
    {
   
        var queryHandlerTypes = assembliesWithHandlers
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                            )
                )
            ;
            
        foreach (var queryHandlerType in queryHandlerTypes)
        {
            var queryType = queryHandlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                .GetGenericArguments()[0];
                        
            var resultType = queryHandlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                .GetGenericArguments()[1];
            
            var wrapperType = typeof(MediatRQueryWrapper<,>).MakeGenericType(queryType, resultType);
            var wrappedHandlerType = typeof(MediatRWrappedQueryHandlerAdapter<,>).MakeGenericType(queryType, resultType);
            var genType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<>).MakeGenericType(resultType));
            
            services.TryAddTransient(genType, queryHandlerType);
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }
         
        return services;
    }
}