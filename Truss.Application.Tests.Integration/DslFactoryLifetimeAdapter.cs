using Truss.Testing.Dsl;

namespace Truss.Application.Tests.Integration;

public sealed class DslFactoryLifetimeAdapter : IAsyncLifetime
{
    public DslFactory DslFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DslFactory.InitializeAsync([ typeof(EntityFrameworkTests).Assembly ]);
    }

    public async Task DisposeAsync()
    {
        await DslFactory.DisposeAsync();
    }
}