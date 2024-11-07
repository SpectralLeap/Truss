using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.BaseServices.Buses;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Installation;
using Truss.Monads.Results;

namespace Truss.ServiceInstallations;

internal sealed class BaseServiceInstallation : IServiceInstallation
{
    public Result<Nil> Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDomainEventBus, DomainEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddTransient<IQueryBus, QueryBus>()
            ;

        return Result.Success();
    }
}