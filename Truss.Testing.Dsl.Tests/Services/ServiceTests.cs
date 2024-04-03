namespace Truss.Testing.Dsl.Tests.Services;

public sealed class ServiceTests
{
    private readonly DslFactory _factoryFixture = new();
    
    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryFixture.GetDsl<DslWithOverrides>(tags: "admin");
        var userSystem = _factoryFixture.GetDsl<DslWithOverrides>();
    
        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
        
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryFixture.GetDsl<DslWithOverrides>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryFixture.GetDsl<DslWithOverrides>();
    
        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
            
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }

    [Fact]
    public void ThrowsWhenTheDslRequestsADependencyItDidNotRegister()
    {
       Assert.Throws<DslServicesNotRegisteredException>(() => _factoryFixture.GetDsl<DslRequestingUnregisteredDependency>());
    }

    [Fact]
    public void ThrowsWhenBaseServiceDefinitionIsNotStatic()
    {
       Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetDsl<DslWithNonStaticBaseServices>());
    }
    
    [Fact]
    public void ThrowsWhenOverrideServiceDefinitionIsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetDsl<DslWithNonStaticOverrideService>());
    }
    
    [Fact]
    public void GivesTheNameOfTheNonStaticMember()
    {
        var msg = Assert.Throws<DslServicesNotStaticException>(() => _factoryFixture.GetDsl<DslWithNonStaticBaseServices>())
            .Message;

        Assert.Contains("NotStatic", msg);
    }

    [Fact]
    public void ThrowsIfNotRightType()
    {
        Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryFixture.GetDsl<DslWithWrongCollectionType>());
    }
    
    [Fact]
    public void GivesTheNameOfIncorrectlyTypedMember()
    {
        var msg = Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryFixture.GetDsl<DslWithWrongCollectionType>())
            .Message;

        Assert.Contains("IncorrectType", msg);
    }
}