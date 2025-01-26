using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Interface for installing services
/// </summary>
public interface IServiceInstallation
{
    /// <summary>
    /// Install a service to the service collection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    public void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    );
}