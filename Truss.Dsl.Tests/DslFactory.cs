using Microsoft.Extensions.DependencyInjection;
using Truss.Core;

namespace Truss.Dsl.Tests;


public sealed class DslFactory(IServiceCollection baseCollection) : IDisposable
{
    private readonly IServiceCollection _baseCollection = baseCollection;
    private readonly List<ServiceProvider> _providers = [];

    public DslLayer GetConfigurationA()
    {
        var provider = new ServiceCollection()
                .AddTruss(c => 
                    c.UsingAssembly(typeof(DslLayer).Assembly))
                .AddSingleton<DslLayer>()
                .BuildServiceProvider()
            ;
        
        _providers.Add(provider);
        return provider.GetService<DslLayer>()!;
    }
    
    public DslLayer GetConfigurationB()
    {
        var provider = new ServiceCollection()
                .AddSingleton<DslLayer>()
                .AddSingleton<IntegrationBus>()
                .BuildServiceProvider()
            ;
        
        _providers.Add(provider);
        return provider.GetService<DslLayer>()!;
    }

    public void Dispose()
    {
        foreach (var provider in _providers)
        {
            provider.Dispose();
        }
    }
}