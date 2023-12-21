using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;

namespace Truss.Dsl;

public sealed class DslNotRegisteredException(Type t) : Exception($"The service {t.Name} was not registered");

public sealed class DslTagNotFoundException(string tag) : Exception($"The tag {tag} was not found");

public sealed class DomainDslFactory : IDisposable
{
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];
    
    public TDsl GetDsl<TDsl>(string? tag = null, string? id = null)
    {
        id ??= Guid.NewGuid().Take(5);

        if (_activeProviders.TryGetValue(id, out var provider)) return provider.GetService<TDsl>()!;
        
        return Activate<TDsl>(GetServices<TDsl>(tag), id);
    }

    private IServiceCollection GetServices<TDsl>(string? tag)
    {
        var types = typeof(TDsl).Assembly
            .GetTypes();

        var baseCollectionType = types
            ;

        if (baseCollectionType is null) throw new ArgumentException("Base collection is null");
        
        var collectionCopy = new ServiceCollection()
                .AddSingleton<IIntegrationBus, IntegrationBus>()
            ;
        
        var driverType = typeof(Driver<>);
                
        var driverDeclarations = types
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == driverType))
            .ToList();
         
        foreach (var declaration in driverDeclarations)
        {
            var interfaceType = declaration.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == driverType);
                     
            collectionCopy.AddTransient(interfaceType, declaration);
        }
        
        return collectionCopy;
    }

    private TDsl Activate<TDsl>(IServiceCollection serviceCollection, string id)
    {
        var provider = serviceCollection.BuildServiceProvider();
        
        _activeProviders.Add(id, provider);
        
        var service = provider.GetService<TDsl>();

        if (service is null) throw new DslNotRegisteredException(typeof(TDsl));

        return service;
    }

    private bool _disposing;
    public void Dispose()
    {
        if (_disposing) return;
        
        _disposing = true;
        
        foreach (var provider in _activeProviders.Values)
        {
            if (provider is IDisposable disposable) disposable.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }
}