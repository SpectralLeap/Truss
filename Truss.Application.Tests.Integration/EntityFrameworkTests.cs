using Truss.Testing.Dsl;

namespace Truss.Application.Tests.Integration;

public sealed class EntityFrameworkTests 
    : IClassFixture<DslFactoryLifetimeAdapter>
{
    private readonly DslFactory _factory;

    public EntityFrameworkTests(DslFactoryLifetimeAdapter factory)
    {
        _factory = factory.DslFactory;
    }
    
    [Fact]
    public async Task CanUseDslAndDriver()
    {
        var dsl = _factory.GetDsl<AutoShopDsl>();

        await dsl.AddAndGetShopUsingDslAndDriver("name: phillips");
    }
    
    [Fact]
    public void CanUseDependenciesDirectlyOnTheDsl()
    {
        var dsl = _factory.GetDsl<AutoShopDsl>();
    
        dsl.AddAndGetShopOnDsl();
    }
}