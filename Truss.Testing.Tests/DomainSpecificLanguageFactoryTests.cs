using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests;

public sealed class DomainSpecificLanguageFactoryTests 
{
    private readonly DomainSpecificLanguageFactory _factoryDomainSpecificLanguage = new();

    [Fact]
    public void ThrowsWhenTagNotFound()
    {
        Assert.Throws<DslTagNotFoundException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguage>(tags: "not a tag"));
    }
   
    [Fact]
    public void GettingDifferentIdDoesNotShareProvider()
    {
        var instance1 = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguage>(id: "1");
        var instance2 = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguage>(id: "2");
                
        Assert.NotEqual(instance1.ProviderGuid, instance2.ProviderGuid);
    }
    
    [Fact]
    public void OverridesAreApplied()
    {
        Assert.True(_factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguage>(tags: "admin").IsAdmin);
        Assert.False(_factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguage>().IsAdmin);
    }

    [Fact]
    public void OverridesAreAppliedWhenProperties()
    {
        Assert.True(_factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguageWithPropertiesDefined>(tags: "admin").IsAdmin);
        Assert.False(_factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguageWithPropertiesDefined>().IsAdmin);
    }

    [Fact]
    public void ThrowsWhenServiceDefinitionsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<RegisteredDomainSpecificLanguageWithNonStaticDefinitions>());
    }

}