using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Drivers;
using Truss.Testing.Services;

namespace Truss.Testing;

internal sealed class FixtureManager : IAsyncDisposable
{
    // This can change locations after dependency injection version 5.0 so using
    // reflection to get it
    private static MethodInfo? _serviceProviderBuilder;
    private readonly ProxyGenerator _proxyGenerator;
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];
    private readonly SharedDependencyManager? _sharedDependencyManager;

    public FixtureManager(
        ProxyGenerator proxyGenerator
    )
    {
        _proxyGenerator = proxyGenerator;
    }

    public FixtureManager(
        SharedDependencyManager sharedDependencyManager,
        ProxyGenerator proxyGenerator
    )
    {
        _sharedDependencyManager = sharedDependencyManager;
        _proxyGenerator = proxyGenerator;
    }

    public TDsl ClassProxyWithTarget<TDsl>(string id, string[] tags) where TDsl : Fixture
    {
        var provider = _activeProviders.TryGetValue(id, out var activeProvider)
            ? activeProvider
            : Activate(GetServices<TDsl>(tags), id);

        var interceptor = provider.GetService<FixtureInterceptor>()!;

        var constructorArguments = ResolveConstructorArgumentsFor<TDsl>(provider);

        try
        {
            var instance = ActivatorUtilities.CreateInstance<TDsl>(provider);

            if (typeof(TDsl).IsSealed) return instance;
            
            var proxy = (TDsl) _proxyGenerator.CreateClassProxyWithTarget(
                typeof(TDsl),
                instance,
                constructorArguments,
                interceptor
            );

            return proxy;
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.ToLower().StartsWith("unable to resolve service"))
                throw new DslServicesNotRegisteredException(typeof(TDsl));

            throw;
        }
    }

    private object[] ResolveConstructorArgumentsFor<T>(IServiceProvider provider) where T : class
    {
        var constructorInfo = typeof(T).GetConstructors().OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();
        
        if (constructorInfo == null)
        {
            return Array.Empty<object>();
        }

        return constructorInfo.GetParameters().Select(p => provider.GetService(p.ParameterType)).ToArray();
    }

    private IServiceCollection GetServices<TDsl>(params string[] tags) where TDsl : class
    {
        var collectionCopy = new ServiceCollection()
                .AddSingleton<DriverDispatcher>()
                .AddSingleton<FixtureInterceptor>()
            ;

        collectionCopy.AddSingleton<TDsl>();

        var serviceDefinitions = ServiceDefinitions.For<TDsl>();

        if (_sharedDependencyManager is not null)
        {
            collectionCopy.Load(_sharedDependencyManager!.SharedDependencyAdapters!);
        }

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
            var type = implementation.BaseType!;

            collectionCopy.AddTransient(type, implementation);
        }

        return collectionCopy;
    }

    private IServiceProvider Activate(IServiceCollection services, string id)
    {
        _serviceProviderBuilder ??= new[]
            {
                Assembly.Load("Microsoft.Extensions.DependencyInjection"),
                Assembly.Load("Microsoft.Extensions.DependencyInjection.Abstractions"),
            }
            .SelectMany(assembly =>
                assembly.GetTypes()
                    .Where(type => type.GetMethods().Any(method => method.Name.Contains("BuildServiceProvider"))))
            .Select(type => type.GetMethod("BuildServiceProvider", [typeof(IServiceCollection)]))
            .FirstOrDefault();

        var provider = (IServiceProvider)_serviceProviderBuilder!.Invoke(services, [services]);

        _activeProviders.Add(id, provider);

        return provider;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var provider in _activeProviders.Values)
        {
            switch (provider)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    continue;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}


/// <summary>
/// Represents an exception that is thrown when a DSL tag is not found among the available tags.
/// </summary>
public sealed class DslTagNotFoundException(string tag, IEnumerable<string> availableTags) 
    : Exception($"The override tag {tag} was not in the available tags [{string.Join(", ", availableTags)}]");

/// <summary>
/// Represents an exception that is thrown when a DSL Collection is not of type IServiceCollection.
/// </summary>
public sealed class DslServiceDefinitionIsNotIServiceCollectionException(MemberInfo info) 
    : Exception($"{info.Name} is not an IServiceCollection. All service definitions must be defined as IServiceCollection");

/// <summary>
/// Represents an exception that is thrown when a Dsl service is not defined as Static
/// </summary>
public sealed class DslServicesNotStaticException(MemberInfo info) 
    : Exception($"The service definition {info.Name} is not static. Dsl Services must be a static field or property");

/// <summary>
/// The exception that is thrown when services requested by a specific type were not registered.
/// </summary>
public sealed class DslServicesNotRegisteredException(Type type) 
    : Exception($"Services requested by {type.Name} were not registered on the type." 
                + $" Assure all types requested for are registered in a {nameof(BaseServicesAttribute)}");

