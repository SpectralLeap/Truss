using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Interface for installing services
/// </summary>
public abstract class ServiceInstaller
{
   
    /// <summary>
    /// Install services to the service collection
    /// </summary>
    /// <param name="services">
    /// The service collection to add the service to
    /// </param>
    /// <param name="configuration">
    /// The configuration to use
    /// </param>
    public virtual void Install(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        Install(services, configuration, Array.Empty<Assembly>());
    }
    
    /// <summary>
    /// Install services to the service collection
    /// </summary>
    /// <param name="services">
    /// The service collection to add the service to
    /// </param>
    /// <param name="configuration">
    /// The configuration to use
    /// </param>
    /// <param name="assemblies">
    /// The assemblies to scan
    /// </param>
    public virtual void Install(
        IServiceCollection services,
        IConfiguration configuration,
        IReadOnlyCollection<Assembly> assemblies
    )
    {
            
    }
        
}