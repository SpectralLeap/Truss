using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Installation;

/// <summary>
/// Base class for a module to be installed by Truss
/// </summary>
public abstract class Module : IModule
{
    /// <inheritdoc />
    public virtual string Name { get; } = "";

    /// <inheritdoc />
    public  IReadOnlyCollection<Assembly> Assemblies => _assemblies;
    
    private readonly List<Assembly> _assemblies = new();

    /// <summary>
    /// Adds an assembly to be scanned for installation.
    ///
    /// The assembly declaring the module is automatically added.
    /// </summary>
    /// <param name="assembly"></param>
    protected void AddAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
    }
    
    /// <inheritdoc />
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        
    }
    
    /// <inheritdoc />
    public void Initialize(IServiceProvider serviceProvider)
    {
    }
}