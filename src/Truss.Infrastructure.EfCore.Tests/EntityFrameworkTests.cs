using Truss.Testing;

namespace Truss.Infrastructure.EfCore.Tests;

[Collection(nameof(DriverFactoryLifetimeAdapter))]
public sealed class EntityFrameworkTests 
{
    private readonly DriverFactory _factory;

    public EntityFrameworkTests(DriverFactoryLifetimeAdapter factory)
    {
        _factory = factory.DriverFactory;
    }
    
    [Fact]
    public async Task CanUseDslAndDriver()
    {
        var dsl = await _factory.GetDriverAsync<AutoShopDriver>();

        await dsl.AddAndGetShopUsingDslAndDriver("name: phillips");
    }
    
    [Fact]
    public async Task CanUseDependenciesDirectlyOnTheDsl()
    {
        var dsl = await _factory.GetDriverAsync<AutoShopDriver>();

        dsl.AddAndGetShopOnDsl();
    }
}