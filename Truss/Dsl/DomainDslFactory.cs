using Castle.DynamicProxy;
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
    private static readonly ProxyGenerator ProxyGenerator = new();
    
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];

    /// <summary>
    /// Retrieves an instance of a DSL (Domain Specific Language) based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDsl">The type of the DSL to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the DSL instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply DSL service overrides.</param>
    /// <returns>An instance of the specified DSL type.</returns>
    public TDsl GetDsl<TDsl>(string? id = null, params string[] tags) where TDsl : class
    {
        id ??= Guid.NewGuid().Take(5);

        var provider = _activeProviders.TryGetValue(id, out var activeProvider) ? activeProvider : Activate(GetServices<TDsl>(tags), id);

        var interceptor = provider.GetService<DslInterceptor>()!;
        
        var constructorArguments = ResolveConstructorArgumentsFor<TDsl>(provider);
        
        var instance = ActivatorUtilities.CreateInstance<TDsl>(provider);
        
        return (TDsl)ProxyGenerator.CreateClassProxyWithTarget(typeof(TDsl), instance, constructorArguments, interceptor);
    } 

    private object[] ResolveConstructorArgumentsFor<T>(IServiceProvider provider) where T : class
    {
        var constructorInfo = typeof(T).GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        if (constructorInfo == null)
        {
            return Array.Empty<object>();
        }

        return constructorInfo.GetParameters().Select(p => provider.GetService(p.ParameterType)).ToArray();
    }

    private IServiceCollection GetServices<TDsl>(params string[] tags) where TDsl : class
    {
        var collectionCopy = new ServiceCollection()
                .AddSingleton<IIntegrationBus, IntegrationBus>()
                .AddSingleton<DslInterceptor>()
            ;

        collectionCopy.AddSingleton<TDsl>();
        
        var serviceDefinitions = ServiceDefinitions.For<TDsl>();

        collectionCopy.Load(serviceDefinitions.GetBaseServices());
        collectionCopy.Load(serviceDefinitions.GetOverrideServices(tags));

        var driverType = typeof(Driver<>);

        var driverImplementations = typeof(TDsl).Assembly
            .GetTypes()
            .Where(type => type.BaseType is not null && type.BaseType.IsGenericType)
            .Where(type => type.BaseType!.GetGenericTypeDefinition() == driverType)
            .ToList();

        foreach (var implementation in driverImplementations)
        {
            var type = implementation.BaseType;
                                 
            collectionCopy.AddTransient(type, implementation);
        }

       
        return collectionCopy;
    }

    private IServiceProvider Activate(IServiceCollection serviceCollection, string id)
    {
        var provider = serviceCollection.BuildServiceProvider();
        
        _activeProviders.Add(id, provider);

        return provider;
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
