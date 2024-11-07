using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;
using Truss.Monads.Results;

namespace Truss.Infrastructure.ServiceInstallations;

internal sealed class MediatrServiceInstallation : IServiceInstallation
{
    public Result<Nil> Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(assemblies.ToArray()));

        return Result.Success();
    }
}