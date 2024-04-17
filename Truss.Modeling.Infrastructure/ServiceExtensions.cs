using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.DomainEvents;

namespace Truss.Modeling.Infrastructure;

public static class ServiceExtensions
{
    public static void AddTruss(
        this IServiceCollection services,
        Assembly[] assemblies
    )
    {
        var model = new TrussDependencyModel(
            services,
            assemblies
        );

        model.CloseAllTypesOf(typeof(IDomainEventHandler<>));
        model.CloseAllTypesOf(typeof(ICommandHandler<>));
        model.CloseAllTypesOf(typeof(IQueryHandler<,>));
        model.CloseAllTypesOf(typeof(IChangeEventHandler<>));
    }
}