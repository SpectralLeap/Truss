using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Application.Installation;

/// <summary>
/// Interface for a module to be installed by Truss
/// </summary>
public interface IModule
{
    /// <summary>
    /// <p>
    /// Defines the services to install for the module.
    /// Typically, these are internal types required to run
    /// but can also apply to infrastructure.
    /// </p>
    /// <p>
    /// If only using internal types this can be defined in an
    /// Application or Core project.
    /// </p>
    /// <p>
    /// If adding infrastructure the module should be defined in
    /// a separate project to avoid circular dependencies.
    /// </p>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public void Define(
        IServiceCollection services,
        IConfiguration configuration
    );
}