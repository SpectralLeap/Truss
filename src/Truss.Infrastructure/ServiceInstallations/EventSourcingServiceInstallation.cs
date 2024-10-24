using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Infrastructure.DefaultServices.EventSourcingServices;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Installation;
using Truss.Monads.Results;

namespace Truss.Infrastructure.ServiceInstallations;

internal sealed class EventSourcingServiceInstallation : IServiceInstallation
{
    public Result<Nil> Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services
            .AddSingleton(new ChangeEventTypeMap(assemblies.ToArray()))
            .AddSingleton<ChangeEventSerializer>()
            .AddSingleton<ChangeEventDeserializer>()
            .AddTransient<IAggregateEventStreamWriter, AggregateEventStreamWriter>()
            .AddTransient<IAggregateEventStreamReader, AggregateEventStreamReader>();
        
        return Result.Success();
    }
}