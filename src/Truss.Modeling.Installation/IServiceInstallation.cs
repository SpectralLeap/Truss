using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Monads.Results;

namespace Truss.Modeling.Installation;

public interface IServiceInstallation
{
    public Result<Nil> Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    );
}