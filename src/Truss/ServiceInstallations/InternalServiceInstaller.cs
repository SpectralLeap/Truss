using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.ConcreteServices;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Domain.Events;
using Truss.Modeling.Installation;

namespace Truss.ServiceInstallations;

internal sealed class InternalServiceInstaller
    : ServiceInstaller
{
    public override void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDomainEventBus, DomainEventBus>()
            .AddTransient<ICommandBus, CommandBus>()
            .AddTransient<IQueryBus, QueryBus>();
    }
}