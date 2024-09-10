using Truss.Testing;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class EntityFrameworkTests 
    : IClassFixture<DriverFactoryLifetimeAdapter>
{
    private readonly DriverFactory _factory;

    public EntityFrameworkTests(DriverFactoryLifetimeAdapter factory)
    {
        _factory = factory.DriverFactory;
    }
    
    [Fact]
    public async Task CanUseDslAndDriver()
    {
        var dsl = _factory.GetDriver<AutoShopDriver>();

        await dsl.AddAndGetShopUsingDslAndDriver("name: phillips");
    }
    
    [Fact]
    public void CanUseDependenciesDirectlyOnTheDsl()
    {
        var dsl = _factory.GetDriver<AutoShopDriver>();
    
        dsl.AddAndGetShopOnDsl();
    }
}