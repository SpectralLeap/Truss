using System.Reflection;
using Truss.Testing;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class DriverFactoryLifetimeAdapter : IAsyncLifetime
{
    public DriverFactory DriverFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DriverFactory.InitializeAsync(new List<Assembly> {typeof(EntityFrameworkTests).Assembly}.AsReadOnly());
    }

    public async Task DisposeAsync()
    {
        await DriverFactory.DisposeAsync();
    }
}