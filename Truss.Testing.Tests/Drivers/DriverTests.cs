namespace Truss.Testing.Tests.Drivers;

public sealed class DriverTests
{
    private readonly DriverFactory _factoryDriver = new();
     
    [Fact]
    public async Task TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryDriver.GetDriver<SutDriver>();
        
        await system.RegisterUser();
        
        system.AssertRegistered();
    }


    [Fact]
    public async Task TheActCanBeOverridenAndStillPerformsDriverFunctions()
    {
        var dslWithActOverride = _factoryDriver.GetDriver<SutDriverOverridingAct>();
        await dslWithActOverride.RegisterUser();
        
        dslWithActOverride.AssertBothActionsHappened();
    }
}