using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.DomainEvents;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Domain.EventSourcing;
using Truss.Modeling.Infrastructure.MediatR.Adapters;
using Truss.Modeling.Infrastructure.MediatR.Buses;
using Truss.Modeling.Infrastructure.MediatR.Wrappers;
using Truss.Monads.Results;

namespace Truss.Modeling.Infrastructure.MediatR;


public sealed class TrussMediatrInfrastructureInstaller : ITrussServiceInstaller
{
    public void InstallServices(TrussDependencyModel trussDependencyModel)
    {
        trussDependencyModel.Add((services, assemblies) =>
            {
#if NETSTANDARD2_0
                services.AddMediatR(c =>
                        c.RegisterServicesFromAssemblies([..assemblies]))
                    .AddAdapters(assemblies)
                    ;
#endif
#if NETFRAMEWORK
                services.AddMediatR([..assemblies])
                    .AddDynamicMediatRHandlers(assemblies)
                    ;
#endif
            })
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDomainEventBus, DomainEventBus>()
            .AddTransient<IChangeEventBus, ChangeEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddTransient<IQueryBus, QueryBus>();
    }
}

internal static class ServiceExtensions
{
    public static IServiceCollection AddAdapters(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        return services
                .AddDomainEventWrappers(assemblies)
                .AddSingleArityCommandWrappers(assemblies)
                .AddDoubleArityWrappers(assemblies)
                .AddQueryWrappers(assemblies)
                .AddChangeEventWrappers()
            ;
    }

    private static IServiceCollection AddChangeEventWrappers(
        this IServiceCollection services
    )
    {
        services.TryAddTransient<IChangeEventHandler<ChangeEvent>, ChangeEventHandler>();
        services.TryAddTransient<INotificationHandler<ChangeEventWrapper<ChangeEvent>>,
            ChangeEventHandlerAdapter<ChangeEvent>>();

        return services;
    }
    
    private static IServiceCollection AddDomainEventWrappers(
        this IServiceCollection services,
        Assembly[] assembliesWithWrappers
    )
    {
        var domainEventHandlerTypes = assembliesWithWrappers
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
                     
            var wrapperType = typeof(DomainEventWrapper<>).MakeGenericType(domainEventType);
            var wrappedHandlerType = typeof(DomainEventHandlerAdapter<>).MakeGenericType(domainEventType);
            var wrappedGenType = typeof(INotificationHandler<>).MakeGenericType(wrapperType);
         
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }
    
        return services;
    }
     
    private static IServiceCollection AddSingleArityCommandWrappers(
        this IServiceCollection services,
        Assembly[] assembliesWithWrappers
    )
    {
        var handlerTypes = assembliesWithWrappers
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
            
            var wrapperType = typeof(CommandWrapper<>).MakeGenericType(commandType);
            var wrappedHandlerType = typeof(CommandHandlerAdapter<>).MakeGenericType(commandType);
            var genType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<Nil>));

            services.TryAddTransient(genType, handlerType);
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }

        return services;
    }

    private static IServiceCollection AddDoubleArityWrappers(
        this IServiceCollection services,
        Assembly[] assembliesWithWrappers
    )
    {
        var commandHandlerTypes = assembliesWithWrappers
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
     
            var wrapperType = typeof(CommandWrapper<,>).MakeGenericType(commandType, resultType);
            var wrappedHandlerType = typeof(CommandHandlerAdapter<,>).MakeGenericType(commandType, resultType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<>).MakeGenericType(resultType));
     
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }

        return services;
    }
    
    private static IServiceCollection AddQueryWrappers(
        this IServiceCollection services,
        Assembly[] assembliesWithWrappers
    )
    {
   
        var queryHandlerTypes = assembliesWithWrappers
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
            
            var wrapperType = typeof(QueryWrapper<,>).MakeGenericType(queryType, resultType);
            var wrappedHandlerType = typeof(QueryHandlerAdapter<,>).MakeGenericType(queryType, resultType);
            var wrappedGenType = typeof(IRequestHandler<,>).MakeGenericType(wrapperType, typeof(Result<>).MakeGenericType(resultType));
            
            services.TryAddTransient(wrappedGenType, wrappedHandlerType);
        }
         
        return services;
    }
}