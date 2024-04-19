using System.Reflection;
using Truss.Modeling.Application;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;

namespace Truss.Modeling.Infrastructure;

public sealed class TrussConfig
{
    internal bool IsEventSourcing => _eventStoreType is not null;
    
    private readonly List<Assembly> _moduleAssemblies = [];
    private readonly List<Assembly> _infrastructureAssemblies = [];
    private Type _eventStoreType;
    private Func<IEventStore> _eventStoreFactory;

    
    public TrussConfig AddModule<TModuleInstaller>()
        where TModuleInstaller : ITrussModuleInstaller
    {
        _moduleAssemblies.Add(typeof(TModuleInstaller).Assembly);
        return this;
    }
    
    public void AddInfrastructure<TInfrastructureInstaller>()
        where TInfrastructureInstaller : ITrussInfrastructureInstaller
    {
        _infrastructureAssemblies.Add(typeof(TInfrastructureInstaller).Assembly);
    }
    
    public void WithEventStore<TEventStore>(
        Func<IEventStore> factory = null
    )
        where TEventStore : IEventStore
    {
        _eventStoreType = typeof(TEventStore);
    }

    internal Assembly[] GetModuleAssemblies()
    {
        return _moduleAssemblies.ToArray();
    }

    internal Assembly[] GetInfrastructureAssemblies()
    {
        return _infrastructureAssemblies.ToArray();
    }

    internal Type GetEventStoreType()
    {
        return _eventStoreType;
    }

    internal Func<IEventStore>? GetEventStoreFactory()
    {
        return _eventStoreFactory;
    }

}