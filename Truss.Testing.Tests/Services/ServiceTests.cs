namespace Truss.Testing.Tests.Services;

public sealed class ServiceTests
{
    private readonly FixtureFactory _factoryFixture = new();
    
    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryFixture.GetFixture<FixtureWithOverrides>(tags: "admin");
        var userSystem = _factoryFixture.GetFixture<FixtureWithOverrides>();
    
        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
        
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryFixture.GetFixture<FixtureWithOverrides>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryFixture.GetFixture<FixtureWithOverrides>();
    
        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
            
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }

    [Fact]
    public void ThrowsWhenTheDslRequestsADependencyItDidNotRegister()
    {
       Assert.Throws<DslServicesNotRegisteredException>(() => _factoryFixture.GetFixture<FixtureRequestingUnregisteredDependency>());
    }

    [Fact]
    public void ThrowsWhenBaseServiceDefinitionIsNotStatic()
    {
       Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetFixture<FixtureWithNonStaticServices>());
    }
    
    [Fact]
    public void ThrowsWhenOverrideServiceDefinitionIsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetFixture<FixtureWithNonStaticOverrideService>());
    }
    
    [Fact]
    public void GivesTheNameOfTheNonStaticMember()
    {
        var msg = Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetFixture<FixtureWithNonStaticServices>())
            .Message;

        Assert.Contains("NotStatic", msg);
    }

    [Fact]
    public void ThrowsIfNotRightType()
    {
        Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryFixture.GetFixture<FixtureWithWrongCollectionType>());
    }
    
    [Fact]
    public void GivesTheNameOfIncorrectlyTypedMember()
    {
        var msg = Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryFixture.GetFixture<FixtureWithWrongCollectionType>())
            .Message;

        Assert.Contains("IncorrectType", msg);
    }
}