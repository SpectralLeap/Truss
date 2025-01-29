using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss.ServiceInstallations;

internal sealed class MediatrServiceInstaller
    : IServiceInstaller
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        if (assemblies.Count == 0)
        {
            assemblies = [GetType().Assembly];
        }

        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(assemblies.ToArray())
        );
    }
}