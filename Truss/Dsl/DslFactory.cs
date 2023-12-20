using Microsoft.Extensions.DependencyInjection;

namespace Truss.Dsl;

public sealed class DslNotRegisteredException(Type t) : Exception($"The service {t.Name} was not registered");

public sealed class DslTagNotFoundException(string tag) : Exception($"The tag {tag} was not found");

public abstract class DslFactory(IServiceCollection baseCollection) : IDisposable
{
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];
    private readonly Dictionary<string, IServiceCollection> _overrideCollections = [];
    
    protected void RegisterOverrideSet(string tag, IServiceCollection services)
    {
        _overrideCollections.Add(tag, services);
    }

    public TDsl GetDsl<TDsl>(string? tag = null, string? id = null) where TDsl : DomainDsl
    {
        id ??= Guid.NewGuid().Take(5);

        if (_activeProviders.TryGetValue(id, out var provider)) return provider.GetService<TDsl>()!;
        
        var collectionCopy = new ServiceCollection();

        collectionCopy.Load(baseCollection)
            .AddTruss(c => c.UsingAssembly(typeof(TDsl).Assembly))
            ;

        return tag is null
            ? Activate<TDsl>(collectionCopy, id)
            : GetDslWithOverrides<TDsl>(tag, id, collectionCopy);
    }

    private TDsl GetDslWithOverrides<TDsl>(string tag, string id, IServiceCollection collectionCopy)
    {
        if (!_overrideCollections.TryGetValue(tag, out var overrides)) throw new DslTagNotFoundException(tag);
     
        collectionCopy.Load(overrides);
            
        return Activate<TDsl>(collectionCopy, id);   
    }

    private TDsl Activate<TDsl>(IServiceCollection serviceCollection, string id)
    {
        
        var provider = serviceCollection.BuildServiceProvider();
        
        _activeProviders.Add(id, provider);
        
        var service = provider.GetService<TDsl>();

        if (service is null) throw new DslNotRegisteredException(typeof(TDsl));

        return service;
    }

    private bool disposing;
    public void Dispose()
    {
        if (disposing) return;
        
        disposing = true;
        
        foreach (var provider in _activeProviders.Values)
        {
            if (provider is IDisposable disposable) disposable.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }
}

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection Load(this IServiceCollection services, IServiceCollection otherServices)
    {
        foreach (var serviceDescriptor in otherServices)
        {
            services.Add(serviceDescriptor);
        }

        return services;
    }
}