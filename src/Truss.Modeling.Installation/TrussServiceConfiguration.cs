using System.Reflection;
using Microsoft.Extensions.Configuration;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Installation;

public sealed class TrussServiceConfiguration
{
    internal bool IsEventSourcing => _eventStoreType is not null;
    internal IConfiguration Configuration { get; private set; }
    
    private readonly List<Assembly> _moduleAssemblies = [];
    private readonly List<Assembly> _infrastructureAssemblies = [];
    private Type? _eventStoreType;
    private Func<IEventStore>? _eventStoreFactory;

    public TrussServiceConfiguration UseConfiguration(IConfiguration configuration)
    {
        Configuration = configuration;
        return this;
    }
    
    public TrussServiceConfiguration InstallModule<TModuleInstaller>()
        where TModuleInstaller : IModule
    {
        _moduleAssemblies.Add(typeof(TModuleInstaller).Assembly);
        return this;
    }

    public TrussServiceConfiguration SetEventStore<TEventStore>(
        Func<IEventStore>? factory = null
    )
        where TEventStore : IEventStore
    {
        _eventStoreType = typeof(TEventStore);
        _eventStoreFactory = factory;
        
        return this;
    }

    internal Assembly[] GetModuleAssemblies()
    {
        return _moduleAssemblies.ToArray();
    }

    internal Assembly[] GetInfrastructureAssemblies()
    {
        return _infrastructureAssemblies.ToArray();
    }

    internal Type? GetEventStoreType()
    {
        return _eventStoreType;
    }

    internal Func<IEventStore>? GetEventStoreFactory()
    {
        return _eventStoreFactory;
    }

}