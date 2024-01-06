namespace Truss.Testing.Dsl.Tests.Core;

public sealed class AssertionTests
{
    private readonly DslFactory _factoryFixture = new();
     
    [Fact]
    public async Task TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryFixture.GetDsl<SutDsl>();
        
        await system.RegisterUser();
        await system.RegisterUser();
        
        system.AssertRegistered();
    }

    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryFixture.GetDsl<SutDsl>(tags: "admin");
        var userSystem = _factoryFixture.GetDsl<SutDsl>();

        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
    
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryFixture.GetDsl<SutDsl>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryFixture.GetDsl<SutDsl>();

        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
        
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }
}