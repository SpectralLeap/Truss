using Truss.Testing.Dsl.Tests.Fixtures;

namespace Truss.Testing.Dsl.Tests;

public sealed class DslFactoryTests
{
    private readonly DslFactory _factoryFixture = new();

    [Fact]
    public void ThrowsWhenTagNotFound()
    {
        Assert.Throws<DslTagNotFoundException>(() => _factoryFixture.GetDsl<RegisteredDsl>(tags: "not a tag"));
    }
   
    [Fact]
    public void GettingDifferentIdDoesNotShareProvider()
    {
        var instance1 = _factoryFixture.GetDsl<RegisteredDsl>(id: "1");
        var instance2 = _factoryFixture.GetDsl<RegisteredDsl>(id: "2");
                
        Assert.NotEqual(instance1.ProviderGuid, instance2.ProviderGuid);
    }

    [Fact]
    public void OverridesAreApplied()
    {
        Assert.True(_factoryFixture.GetDsl<RegisteredDsl>(tags: "admin").IsAdmin);
        Assert.False(_factoryFixture.GetDsl<RegisteredDsl>().IsAdmin);
    }

    [Fact]
    public void OverridesAreAppliedWhenProperties()
    {
        Assert.True(_factoryFixture.GetDsl<RegisteredDslWithPropertiesDefined>(tags: "admin").IsAdmin);
        Assert.False(_factoryFixture.GetDsl<RegisteredDslWithPropertiesDefined>().IsAdmin);
    }

    [Fact]
    public void ThrowsWhenServiceDefinitionsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetDsl<RegisteredDslWithNonStaticDefinitions>());
    }

}