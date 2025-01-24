using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Installation;

/// <summary>
/// Configuration for installing services for Truss
/// </summary>
public sealed class TrussServiceConfiguration
{
    /// <summary>
    /// The <see cref="IModule"/>s to be installed
    /// </summary>
    public IReadOnlyCollection<IModule> Modules => _modules;
    /// <summary>
    /// The <see cref="IServiceInstallation"/>s to be installed
    /// </summary>
    public IReadOnlyCollection<IServiceInstallation> ServiceInstallations => _serviceInstallations;
    /// <summary>
    /// If provided, will log install information
    /// </summary>
    public ILogger? Logger { get; private set; }
    /// <summary>
    /// The <see cref="IConfiguration"/> provided to all installations
    /// </summary>
    public IConfiguration Configuration { get; private set; } = new ConfigurationBuilder().Build();

    private readonly List<IModule> _modules = [];
    
    private readonly List<IServiceInstallation> _serviceInstallations = [];

    /// <summary>
    /// Assigns an <see cref="IConfiguration"/> to read from
    /// </summary>
    /// <param name="configuration">
    /// The configuration to use
    /// </param>
    /// <returns></returns>
    public TrussServiceConfiguration UseConfiguration(
        IConfiguration configuration
    )
    {
        Configuration = configuration;
        return this;
    }

    /// <summary>
    /// Assigns an <see cref="ILogger"/> to log from
    /// </summary>
    /// <param name="logger">
    /// The logger to use
    /// </param>
    /// <returns></returns>
    public TrussServiceConfiguration UseLogger(
        ILogger logger
    )
    {
        Logger = logger;
        return this;
    }
    
    /// <summary>
    /// Register an <see cref="IModule"/> to be installed
    /// </summary>
    /// <typeparam name="TModuleInstaller">
    /// The concrete implementation to install from
    /// </typeparam>
    /// <returns></returns>
    public TrussServiceConfiguration InstallModule<TModuleInstaller>()
        where TModuleInstaller : IModule, new()
    {
        var module = new TModuleInstaller();
        _modules.Add(module);
        return this;
    }

    /// <summary>
    /// Register an <see cref="IServiceInstallation"/> to be installed
    /// </summary>
    /// <typeparam name="T">
    /// The concrete implementation to install from
    /// </typeparam>
    /// <returns></returns>
    public TrussServiceConfiguration AddServiceInstallation<T>()
        where T : IServiceInstallation, new()
    {
        var serviceInstallation = new T();
        _serviceInstallations.Add(serviceInstallation);
        return this;
    }
}