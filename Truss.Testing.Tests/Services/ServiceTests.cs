namespace Truss.Testing.Tests.Services;

public sealed class ServiceTests
{
    private readonly DomainSpecificLanguageFactory _factoryDomainSpecificLanguage = new();
    
    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithOverrides>(tags: "admin");
        var userSystem = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithOverrides>();
    
        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
        
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithOverrides>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithOverrides>();
    
        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
            
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }

    [Fact]
    public void ThrowsWhenTheDslRequestsADependencyItDidNotRegister()
    {
       Assert.Throws<DslServicesNotRegisteredException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage
       <DomainSpecificLanguageRequestingUnregisteredDependency>());
    }

    [Fact]
    public void ThrowsWhenBaseServiceDefinitionIsNotStatic()
    {
       Assert.Throws<DslServicesNotStaticException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage
           <DomainSpecificLanguageWithNonStaticServices>());
    }
    
    [Fact]
    public void ThrowsWhenOverrideServiceDefinitionIsNotStatic()
    {
        Assert.Throws<DslServicesNotStaticException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage
            <DomainSpecificLanguageWithNonStaticOverrideService>());
    }
    
    [Fact]
    public void GivesTheNameOfTheNonStaticMember()
    {
        var msg = Assert.Throws<DslServicesNotStaticException>(() => _factoryDomainSpecificLanguage.GetDomainSpecificLanguage
                <DomainSpecificLanguageWithNonStaticServices>())
            .Message;

        Assert.Contains("NotStatic", msg);
    }

    [Fact]
    public void ThrowsIfNotRightType()
    {
        Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithWrongCollectionType>());
    }
    
    [Fact]
    public void GivesTheNameOfIncorrectlyTypedMember()
    {
        var msg = Assert.Throws<DslServiceDefinitionIsNotIServiceCollectionException>(() =>
            _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<DomainSpecificLanguageWithWrongCollectionType>())
            .Message;

        Assert.Contains("IncorrectType", msg);
    }
}