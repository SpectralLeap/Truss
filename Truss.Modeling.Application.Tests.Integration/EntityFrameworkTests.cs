using Truss.Testing;

namespace Truss.Modeling.Application.Tests.Integration;

public sealed class EntityFrameworkTests 
    : IClassFixture<DslFactoryLifetimeAdapter>
{
    private readonly FixtureFactory _factory;

    public EntityFrameworkTests(DslFactoryLifetimeAdapter factory)
    {
        _factory = factory.FixtureFactory;
    }
    
    [Fact]
    public async Task CanUseDslAndDriver()
    {
        var dsl = _factory.GetFixture<AutoShopFixture>();

        await dsl.AddAndGetShopUsingDslAndDriver("name: phillips");
    }
    
    [Fact]
    public void CanUseDependenciesDirectlyOnTheDsl()
    {
        var dsl = _factory.GetFixture<AutoShopFixture>();
    
        dsl.AddAndGetShopOnDsl();
    }
}