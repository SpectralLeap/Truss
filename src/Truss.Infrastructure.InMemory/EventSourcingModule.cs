using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Infrastructure.InMemory.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Installation;

namespace Truss.Infrastructure.InMemory;

internal sealed class EventSourcingModule : IModule
{
    public void Define(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventStore, InMemoryEventStore>();
    }
}