using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests;

public sealed class FixtureFactoryTests 
{
    private readonly FixtureFactory _factoryFixture = new();

    [Fact]
    public void ThrowsWhenTagNotFound()
    {
        Assert.Throws<DslTagNotFoundException>(() => _factoryFixture.GetFixture<RegisteredFixture>(tags: "not a tag"));
    }
   
    [Fact]
    public void GettingDifferentIdDoesNotShareProvider()
    {
        var instance1 = _factoryFixture.GetFixture<RegisteredFixture>(id: "1");
        var instance2 = _factoryFixture.GetFixture<RegisteredFixture>(id: "2");
                
        Assert.NotEqual(instance1.ProviderGuid, instance2.ProviderGuid);
    }
    
    [Fact]
    public void OverridesAreApplied()
    {
        Assert.True(_factoryFixture.GetFixture<RegisteredFixture>(tags: "admin").IsAdmin);
        Assert.False(_factoryFixture.GetFixture<RegisteredFixture>().IsAdmin);
    }

    [Fact]
    public void OverridesAreAppliedWhenProperties()
    {
        Assert.True(_factoryFixture.GetFixture<RegisteredFixtureWithPropertiesDefined>(tags: "admin").IsAdmin);
        Assert.False(_factoryFixture.GetFixture<RegisteredFixtureWithPropertiesDefined>().IsAdmin);
    }

    [Fact]
    public void ThrowsWhenServiceDefinitionsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetFixture<RegisteredFixtureWithNonStaticDefinitions>());
    }

}