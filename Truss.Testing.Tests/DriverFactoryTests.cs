using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests;

public sealed class DriverFactoryTests
{
    private readonly DriverFactory _driverFactory = new();

    [Fact]
    public void ThrowsWhenTagNotFound()
    {
        Assert.Throws<DslTagNotFoundException>(() => _driverFactory.GetDriver<RegisteredDriver>(tags: "not a tag"));
    }
   
    [Fact]
    public void GettingDifferentIdDoesNotShareProvider()
    {
        var instance1 = _driverFactory.GetDriver<RegisteredDriver>(id: "1");
        var instance2 = _driverFactory.GetDriver<RegisteredDriver>(id: "2");
                
        Assert.NotEqual(instance1.ProviderGuid, instance2.ProviderGuid);
    }
    
    [Fact]
    public void OverridesAreApplied()
    {
        Assert.True(_driverFactory.GetDriver<RegisteredDriver>(tags: "admin").IsAdmin);
        Assert.False(_driverFactory.GetDriver<RegisteredDriver>().IsAdmin);
    }

    [Fact]
    public void OverridesAreAppliedWhenProperties()
    {
        Assert.True(_driverFactory.GetDriver<RegisteredDriverWithPropertiesDefined>(tags: "admin").IsAdmin);
        Assert.False(_driverFactory.GetDriver<RegisteredDriverWithPropertiesDefined>().IsAdmin);
    }

    [Fact]
    public void ThrowsWhenServiceDefinitionsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _driverFactory.GetDriver<RegisteredDriverWithNonStaticDefinitions>());
    }

}