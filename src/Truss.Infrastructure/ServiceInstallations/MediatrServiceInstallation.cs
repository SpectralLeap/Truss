using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;
using Truss.Monads.Results;

namespace Truss.Infrastructure;

internal sealed class MediatrServiceInstallation : IServiceInstallation
{
    public Result<Nil> Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
        
#if NET461 || NET47 || NET48
        services.AddMediatR(assemblies.ToArray());
#else
        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(assemblies.ToArray()));
#endif
        
        return Result.Success();
    }
}