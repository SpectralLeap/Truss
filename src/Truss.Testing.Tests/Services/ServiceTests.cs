namespace Truss.Testing.Tests.Services;

public sealed class ServiceTests
{
    private readonly DriverFactory _factoryDriver = new();
    
    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryDriver.GetDriver<DriverWithOverrides>(tags: "admin");
        var userSystem = _factoryDriver.GetDriver<DriverWithOverrides>();
    
        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
        
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryDriver.GetDriver<DriverWithOverrides>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryDriver.GetDriver<DriverWithOverrides>();
    
        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
            
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }

    [Fact]
    public void ThrowsWhenTheDslRequestsADependencyItDidNotRegister()
    {
       Assert.Throws<DslServicesNotRegisteredException>(() => _factoryDriver.GetDriver
       <DriverRequestingUnregisteredDependency>());
    }

    [Fact]
    public void ThrowsWhenBaseServiceDefinitionIsNotStatic()
    {
       Assert.Throws<DslServicesNotStaticException>(() => _factoryDriver.GetDriver
           <DriverWithNonStaticServices>());
    }
    
    [Fact]
    public void ThrowsWhenOverrideServiceDefinitionIsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryDriver.GetDriver
            <DriverWithNonStaticOverrideService>());
    }
    
    [Fact]
    public void GivesTheNameOfTheNonStaticMember()
    {
        var msg = Assert.Throws<DslServicesNotStaticException>(() => _factoryDriver.GetDriver
                <DriverWithNonStaticServices>())
            .Message;

        Assert.Contains("NotStatic", msg);
    }

    [Fact]
    public void ThrowsIfNotRightType()
    {
        Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryDriver.GetDriver<DriverWithWrongCollectionType>());
    }
    
    [Fact]
    public void GivesTheNameOfIncorrectlyTypedMember()
    {
        var msg = Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryDriver.GetDriver<DriverWithWrongCollectionType>())
            .Message;

        Assert.Contains("IncorrectType", msg);
    }
}