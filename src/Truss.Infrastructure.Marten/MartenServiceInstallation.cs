using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.EventSourcing.Persistence;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure.Marten;

public sealed class MartenServiceInstallation : IServiceInstallation
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services.AddScoped<IAggregateRepository, AggregateRepository>();
    }
}