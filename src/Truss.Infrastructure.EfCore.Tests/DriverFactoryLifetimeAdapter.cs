using Truss.Infrastructure.Tests.Dependencies;
using Truss.Testing;

namespace Truss.Infrastructure.EfCore.Tests;

public sealed class DriverFactoryLifetimeAdapter : IAsyncLifetime
{
    public DriverFactory DriverFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DriverFactory.InitializeAsync([
            typeof(PostgresDependencyAdapter).Assembly,
            typeof(EntityFrameworkTests).Assembly
        ]);
    }

    public async Task DisposeAsync()
    {
        await DriverFactory.DisposeAsync();
    }
}