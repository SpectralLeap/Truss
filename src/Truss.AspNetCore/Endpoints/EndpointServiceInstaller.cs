using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore.Endpoints;

public sealed class EndpointServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<EndpointHandler>();
    }
}
