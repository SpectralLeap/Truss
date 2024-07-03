using System.Reflection;
using Microsoft.Extensions.Configuration;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Area;
using Truss.Modeling.Module;

namespace Truss;

public sealed class TrussServiceConfiguration
{
    internal bool IsEventSourcing => _eventStoreType is not null;
    internal IConfiguration Configuration { get; private set; }
    
    private readonly List<Assembly> _moduleAssemblies = [];
    private Type? _eventStoreType;
    private Func<IServiceProvider, IEventStore>? _eventStoreFactory;

    public TrussServiceConfiguration UseConfiguration(IConfiguration configuration)
    {
        Configuration = configuration;
        return this;
    }

    public TrussServiceConfiguration AddArea<TAreaInstaller>()
        where TAreaInstaller : IAreaInstaller
    {
        return this;
    }
    
    public TrussServiceConfiguration AddModule<TModuleInstaller>()
        where TModuleInstaller : IModuleInstaller
    {
        return this;
    }

    /// <summary>
    /// Registers the Event Store with the application services
    /// </summary>
    /// <param name="factory"></param>
    /// <typeparam name="TEventStore"></typeparam>
    /// <returns></returns>
    public TrussServiceConfiguration UseEventStore<TEventStore>(
        Func<IServiceProvider, IEventStore>? factory = null
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

    internal Type? GetEventStoreType()
    {
        return _eventStoreType;
    }

    internal Func<IServiceProvider, IEventStore>? GetEventStoreFactory()
    {
        return _eventStoreFactory;
    }
}