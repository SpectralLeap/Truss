namespace Truss.Testing.Tests.Drivers;

public sealed class DriverTests
{
    private readonly DomainSpecificLanguageFactory _factoryDomainSpecificLanguage = new();
     
    [Fact]
    public async Task TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<SutDomainSpecificLanguage>();
        
        await system.RegisterUser();
        
        system.AssertRegistered();
    }


    [Fact]
    public async Task TheActCanBeOverridenAndStillPerformsDriverFunctions()
    {
        var dslWithActOverride = _factoryDomainSpecificLanguage.GetDomainSpecificLanguage<SutDomainSpecificLanguageOverridingAct>();
        await dslWithActOverride.RegisterUser();
        
        dslWithActOverride.AssertBothActionsHappened();
    }
}