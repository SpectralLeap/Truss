using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Interface for a module to be installed by Truss
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the name of the module.

    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Assemblies that should be scanned for installation
    /// </summary>
    public IReadOnlyCollection<Assembly> Assemblies { get; }   
    
    /// <summary>
    /// <p>
    /// Defines the module installation.
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
    public void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration
    );

    /// <summary>
    /// Performs any custom initialization logic for the module after the application has started.
    /// This can be used to perform tasks like seeding data, registering runtime services, or triggering startup behaviors.
    /// </summary>
    /// <param name="serviceProvider">The application's service provider.</param>
    public void Initialize(
        IServiceProvider serviceProvider
    );
}