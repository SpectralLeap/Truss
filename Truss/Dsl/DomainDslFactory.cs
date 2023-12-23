using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;

namespace Truss.Dsl;

/// <summary>
/// Represents an exception that is thrown when a DSL service is not registered.
/// </summary>
public sealed class DslNotRegisteredException(Type t) 
    : Exception($"The service {t.Name} was not registered");

/// <summary>
/// Represents an exception that is thrown when a DSL tag is not found among the available tags.
/// </summary>
public sealed class DslTagNotFoundException(string tag, IEnumerable<string> availableTags) 
    : Exception($"The override tag {tag} was not in the available tags [{string.Join(", ", availableTags)}]");

/// <summary>
/// Represents an exception that is thrown when a DSL Collection is not of type IServiceCollection.
/// </summary>
public sealed class DslServiceCollectionNotServiceCollectionException() 
    : Exception("All Base Collections must be defined as IServiceCollection");

/// <summary>
/// Represents an exception that is thrown when a Dsl service is not defined as Static
/// </summary>
public sealed class DslServicesNotStaticException() 
    : Exception("Dsl Services must be defined as Static");

/// <summary>
/// Represents a factory for creating DSL (Domain-Specific Language) instances.
/// </summary>
public sealed class DomainDslFactory : IDisposable
{
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];

    /// <summary>
    /// Retrieves an instance of a DSL (Domain Specific Language) based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDsl">The type of the DSL to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the DSL instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to filter the available DSL instances.</param>
    /// <returns>An instance of the specified DSL type.</returns>
    public TDsl GetDsl<TDsl>(string? id = null, params string[] tags) where TDsl : class
    {
        id ??= Guid.NewGuid().Take(5);
        
        if (_activeProviders.TryGetValue(id, out var provider)) return provider.GetService<TDsl>()!;
        
        return Activate<TDsl>(GetServices<TDsl>(tags), id);
    }

    private IServiceCollection GetServices<TDsl>(params string[] tags) where TDsl : class
    {
        var collectionCopy = new ServiceCollection()
                .AddSingleton<IIntegrationBus, IntegrationBus>()
                .AddSingleton<TDsl>()
            ;

        var serviceDefinitions = ServiceDefinitions.For<TDsl>();

        collectionCopy.Load(serviceDefinitions.GetBaseServices());
        collectionCopy.Load(serviceDefinitions.GetOverrideServices(tags));

        var driverType = typeof(Driver<>);
                
        var driverDeclarations = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType 
                          && i.GetGenericTypeDefinition() == driverType))
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

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposing) return;
        
        _disposing = true;
        
        foreach (var provider in _activeProviders.Values)
        {
            if (provider is IDisposable disposable) disposable.Dispose();
        }
    }
}
