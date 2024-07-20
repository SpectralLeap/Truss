using Truss.Testing;

namespace ExampleApplication.AcceptanceTests;

public sealed class WebApiTests
{
    private readonly FixtureFactory _fixture = new();
    
    [Fact]
    public async Task UserCanLogin()
    {
        var dsl = _fixture.GetFixture<ExampleServiceFixture>();
        
        await dsl.AssertHeartbeat();
    }
}