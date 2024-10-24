using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Installation;

public sealed class TrussServiceConfiguration
{
    public IReadOnlyCollection<IModule> Modules => _modules;
    public IReadOnlyCollection<IServiceInstallation> ServiceInstallations => _serviceInstallations;
    
    public ILogger? Logger { get; private set; }

    public bool IsEventSourcing => EventStoreType is not null;

    public Type? EventStoreType { get; private set; }

    public Func<IEventStore>? EventStoreFactory { get; private set; }

    public IConfiguration Configuration { get; private set; } 
        = new ConfigurationBuilder().Build();

    private readonly List<IModule> _modules = [];
    
    private readonly List<IServiceInstallation> _serviceInstallations = [];

    public TrussServiceConfiguration UseConfiguration(
        IConfiguration configuration
    )
    {
        Configuration = configuration;
        return this;
    }

    public TrussServiceConfiguration UseLogger(
        ILogger logger
    )
    {
        Logger = logger;
        return this;
    }
    
    public TrussServiceConfiguration InstallModule<TModuleInstaller>()
        where TModuleInstaller : IModule, new()
    {
        var module = new TModuleInstaller();
        _modules.Add(module);
        return this;
    }

    public TrussServiceConfiguration UsingEventStore<TEventStore>(
        Func<IEventStore>? factory = null
    )
        where TEventStore : IEventStore
    {
        EventStoreType = typeof(TEventStore);
        EventStoreFactory = factory;
        
        return this;
    }

    public TrussServiceConfiguration AddServiceInstallation<T>()
        where T : IServiceInstallation, new()
    {
        var serviceInstallation = new T();
        _serviceInstallations.Add(serviceInstallation);
        return this;
    }
}