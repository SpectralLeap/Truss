using Truss.Dsl;
using Truss.Tests.Dsl.Fixtures;

namespace Truss.Tests.Dsl;

public sealed class DomainDslFactoryTests
{
    private readonly DomainDslFactory _factoryFixture = new();

    [Fact]
    public void ThrowsWhenTagNotFound()
    {
        Assert.Throws<DslTagNotFoundException>(() => _factoryFixture.GetDsl<RegisteredDsl>(tag: "not a tag"));
    }

    [Fact]
    public void UsesSameInstanceWhenSameId()
    {
        var instance1 = _factoryFixture.GetDsl<RegisteredDsl>(id: "1");
        var instance2 = _factoryFixture.GetDsl<RegisteredDsl>(id: "1");
        
        Assert.Same(instance1, instance2);
    }
    
    [Fact]
    public void GettingDifferentTypeWithSameSharesProvider()
    {
        var instance1 = _factoryFixture.GetDsl<RegisteredDsl>(id: "1");
        var instance2 = _factoryFixture.GetDsl<OtherRegisteredDsl>(id: "1");
            
        Assert.Equal(instance1.Guid, instance2.Guid);
    }
    
    [Fact]
    public void GettingDifferentIdDoesNotShareProvider()
    {
        var instance1 = _factoryFixture.GetDsl<RegisteredDsl>(id: "1");
        var instance2 = _factoryFixture.GetDsl<RegisteredDsl>(id: "2");
                
        Assert.NotEqual(instance1.Guid, instance2.Guid);
    }

    [Fact]
    public void OverridesAreApplied()
    {
        Assert.True(_factoryFixture.GetDsl<RegisteredDsl>(tag: "admin").IsAdmin);
        Assert.False(_factoryFixture.GetDsl<RegisteredDsl>().IsAdmin);
    }

}