using Truss.Testing;

namespace ExampleApplication.AcceptanceTests;

public sealed class WebApiTests
{
    private readonly DriverFactory _driver = new();
    
    [Fact]
    public async Task UserCanLogin()
    {
        var dsl = _driver.GetDriver<ExampleServiceDriver>();
        
        await dsl.AssertHeartbeat();
    }
}