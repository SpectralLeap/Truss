using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing;

internal sealed class DriverManager : IAsyncDisposable
{
    // This can change locations after dependency injection version 5.0 so using
    // reflection to get it
    private static MethodInfo? _serviceProviderBuilder;
    private readonly ProxyGenerator _proxyGenerator;
    private readonly Dictionary<string, IServiceProvider> _activeProviders = [];
    private readonly SharedDependencyManager? _sharedDependencyManager;

    public DriverManager(
        ProxyGenerator proxyGenerator
    )
    {
        _proxyGenerator = proxyGenerator;
    }

    public DriverManager(
        SharedDependencyManager sharedDependencyManager,
        ProxyGenerator proxyGenerator
    )
    {
        _sharedDependencyManager = sharedDependencyManager;
        _proxyGenerator = proxyGenerator;
    }

    public async Task<TDsl> ClassProxyWithTargetAsync<TDsl>(string id, string[] tags) 
        where TDsl : Driver
    {

        var provider = _activeProviders.TryGetValue(id, out var activeProvider)
            ? activeProvider
            : Activate(GetServices<TDsl>(tags), id);

        try
        {
            var instance = ActivatorUtilities.CreateInstance<TDsl>(provider);

            if (instance is IAsyncInitialized asyncInitialized)
            {
                await asyncInitialized.InitializeAsync();
            }

            return instance;
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.ToLower().StartsWith("unable to resolve service"))
                throw new DriverServicesNotRegisteredException(typeof(TDsl));

            throw;
        }
    }

    private IServiceCollection GetServices<TDsl>(params string[] tags) where TDsl : class
    {
        var collectionCopy = new ServiceCollection()
            .AddSingleton<TDsl>();

        var serviceDefinitions = ServiceDefinitions.For<TDsl>();

        if (_sharedDependencyManager is not null)
        {
            collectionCopy.Load(_sharedDependencyManager!.SharedDependencyAdapters);
        }

        collectionCopy.Load(serviceDefinitions.GetBaseServices());
        collectionCopy.Load(serviceDefinitions.GetOverrideServices(tags));

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