using System.Reflection;
using Truss.Testing;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class DslFactoryLifetimeAdapter : IAsyncLifetime
{
    public FixtureFactory FixtureFactory { get; } = new();
    
    public async Task InitializeAsync()
    {
        await FixtureFactory.InitializeAsync(new List<Assembly> {typeof(EntityFrameworkTests).Assembly}.AsReadOnly());
    }

    public async Task DisposeAsync()
    {
        await FixtureFactory.DisposeAsync();
    }
}