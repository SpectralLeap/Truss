using Truss.Infrastructure.Marten.Tests.EventSourcing;
using Truss.Infrastructure.Tests.Dependencies;
using Truss.Testing;

namespace Truss.Infrastructure.Marten.Tests;

public sealed class DriverFactoryLifetimeAdapter : IAsyncLifetime
{
    public DriverFactory DriverFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DriverFactory.InitializeAsync([
            typeof(PostgresDependencyAdapter).Assembly,
            typeof(EventSourcingTests).Assembly
        ]);
    }

    public async Task DisposeAsync()
    {
        await DriverFactory.DisposeAsync();
    }
}