namespace Truss.Testing.Tests.Drivers;

public sealed class DriverTests
{
    private readonly FixtureFactory _factoryFixture = new();
     
    [Fact]
    public async Task TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryFixture.GetFixture<SutFixture>();
        
        await system.RegisterUser();
        
        system.AssertRegistered();
    }


    [Fact]
    public async Task TheActCanBeOverridenAndStillPerformsDriverFunctions()
    {
        var dslWithActOverride = _factoryFixture.GetFixture<SutFixtureOverridingAct>();
        await dslWithActOverride.RegisterUser();
        
        dslWithActOverride.AssertBothActionsHappened();
    }
}