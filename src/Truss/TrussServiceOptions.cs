using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss;

/// <summary>
/// Configuration for installing services for Truss
/// </summary>
public class TrussServiceOptions
{
    /// <summary>
    /// The <see cref="Module"/>s to be installed
    /// </summary>
    public IReadOnlyCollection<Module> Modules => _modules;

    /// <summary>
    /// The <see cref="IServiceCollection"/> that will be used to install services
    /// </summary>
    public IServiceCollection InstallerServices { get; private set; } = new ServiceCollection();
    
    /// <summary>
    /// The <see cref="ServiceProvider"/> that will be used to install services
    /// </summary>
    public ServiceProvider? InstallationServiceProvider { get; private set; }
    
    /// <summary>
    /// The <see cref="IInstallationStep"/>s that will run in the installation pipeline
    /// </summary>
    public IReadOnlyCollection<Type> InstallationSteps => _installationSteps;
    
    /// <summary>
    /// The <see cref="IConfiguration"/> provided to all installations
    /// </summary>
    public IConfiguration Configuration { get; private set; } = new ConfigurationBuilder().Build();

    private readonly List<Module> _modules = [];
    
    private readonly List<Type> _installationSteps = [];

    /// <summary>
    /// Assigns an <see cref="IConfiguration"/> to read from
    /// </summary>
    /// <param name="configuration">
    /// The configuration to use
    /// </param>
    /// <returns></returns>
    public virtual TrussServiceOptions UseConfiguration(
        IConfiguration configuration
    )
    {
        Configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Register an <see cref="Module"/> to be installed
    /// </summary>
    /// <typeparam name="TModuleInstaller">
    /// The concrete implementation to install from
    /// </typeparam>
    /// <returns></returns>
    public TrussServiceOptions InstallModule<TModuleInstaller>()
        where TModuleInstaller : Module, new()
    {
        var module = new TModuleInstaller();
        _modules.Add(module);
        return this;
    }

    public TrussServiceOptions AddInstallationStep<TStep>()
        where TStep : IInstallationStep
    {
        _installationSteps.Add(typeof(TStep));
        return this;
    }
    
    public void BuildServiceProvider()
    {
        InstallationServiceProvider = InstallerServices.BuildServiceProvider();
    }
}
