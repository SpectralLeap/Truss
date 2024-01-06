namespace Truss.Testing.Dsl.Tests.Drivers;

public sealed class DriverTests
{
    private readonly DslFactory _factoryFixture = new();
     
    [Fact]
    public async Task TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryFixture.GetDsl<SutDsl>();
        
        await system.RegisterUser();
        
        system.AssertRegistered();
    }


    [Fact]
    public async Task TheActCanBeOverridenAndStillPerformsDriverFunctions()
    {
        var dslWithActOverride = _factoryFixture.GetDsl<SutDslOverridingAct>();
        await dslWithActOverride.RegisterUser();
        
        dslWithActOverride.AssertBothActionsHappened();
    }
}