using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Drivers;
using Truss.Testing.Services;

namespace Truss.Testing;

internal sealed class DomainSpecificLanguageManager : IAsyncDisposable
{
    // This can change locations after dependency injection version 5.0 so using
    // reflection to get it
    private static MethodInfo? _serviceProviderBuilder;
    private readonly ProxyGenerator _proxyGenerator;
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];
    private readonly SharedDependencyManager? _sharedDependencyManager;

    public DomainSpecificLanguageManager(
        ProxyGenerator proxyGenerator
    )
    {
        _proxyGenerator = proxyGenerator;
    }

    public DomainSpecificLanguageManager(
        SharedDependencyManager sharedDependencyManager,
        ProxyGenerator proxyGenerator
    )
    {
        _sharedDependencyManager = sharedDependencyManager;
        _proxyGenerator = proxyGenerator;
    }

    public async Task<TDsl> ClassProxyWithTargetAsync<TDsl>(string id, string[] tags) where TDsl : DomainSpecificLanguage
    {

        var provider = _activeProviders.TryGetValue(id, out var activeProvider)
            ? activeProvider
            : Activate(GetServices<TDsl>(tags), id);

        var interceptor = provider.GetService<DomainSpecificLanguageInterceptor>()!;

        var constructorArguments = ResolveConstructorArgumentsFor<TDsl>(provider);

        try
        {
            var instance = ActivatorUtilities.CreateInstance<TDsl>(provider);

            if (instance is IAsyncInitialized asyncInitialized)
            {
                await asyncInitialized.InitializeAsync();
            }

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
                .AddSingleton<DomainSpecificLanguageInterceptor>()
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