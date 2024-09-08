using Truss.Testing;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class EntityFrameworkTests 
    : IClassFixture<DomainSpecificLanguageFactoryLifetimeAdapter>
{
    private readonly DomainSpecificLanguageFactory _factory;

    public EntityFrameworkTests(DomainSpecificLanguageFactoryLifetimeAdapter factory)
    {
        _factory = factory.DomainSpecificLanguageFactory;
    }
    
    [Fact]
    public async Task CanUseDslAndDriver()
    {
        var dsl = _factory.GetDomainSpecificLanguage<AutoShopDomainSpecificLanguage>();

        await dsl.AddAndGetShopUsingDslAndDriver("name: phillips");
    }
    
    [Fact]
    public void CanUseDependenciesDirectlyOnTheDsl()
    {
        var dsl = _factory.GetDomainSpecificLanguage<AutoShopDomainSpecificLanguage>();
    
        dsl.AddAndGetShopOnDsl();
    }
}