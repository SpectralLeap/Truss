using ExampleApplication.WebApi;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Truss.Testing;
using Truss.Testing.AspNetCore;
using Truss.Testing.Drivers;
using Truss.Testing.Dsl;
using Truss.Testing.Services;

namespace ExampleApplication.AcceptanceTests;


public sealed class Login;
public sealed class Heartbeat;

public sealed class RequestContext
{
    private readonly List<string?> _requests = new();

    public void Add(string? value)
    {
        _requests.Add(value);
    }
}

public sealed class HeartbeatDriver(HttpClient client, RequestContext requestContext) 
    : Driver<Heartbeat>
{
    public override async Task Drive(DslArgs args)
    {
        var c = new RestClient(client.BaseAddress!);
        var request = new RestRequest("/heartbeat");
        var response = await c.ExecuteAsync(request);

        requestContext.Add(response.Content);
    }
}

public sealed class LoginDriver() : Driver<Login>
{
    public override Task Drive(DslArgs args)
    {
        return Task.CompletedTask;
    }
}

public class ExampleServiceFixture(RequestContext requestContext) : Fixture
{
    [BaseServices]
    public static IServiceCollection ServiceProvider => new ServiceCollection()
        .AddWebServer<Program>()
        .AddSingleton<RequestContext>()
    ;

    public async Task Login(params string[] args)
    {
        await Act(DslArgs.ForAction<Heartbeat>());
    }

    public void AssertHeartbeat()
    {
    }
}

public sealed class WebApiTests
{
    private readonly FixtureFactory _fixture = new();
    
    [Fact]
    public async Task UserCanLogin()
    {
        var dsl = _fixture.GetFixture<ExampleServiceFixture>();
        
        await dsl.Login();
    }
}