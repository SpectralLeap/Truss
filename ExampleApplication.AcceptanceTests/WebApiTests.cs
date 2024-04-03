using ExampleApplication.WebApi;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Truss.Testing.Dsl;
using Truss.Testing.Dsl.Drivers;
using Truss.Testing.Dsl.AspNetCore;
using Truss.Testing.Dsl.Services;

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

public class ExampleServiceDsl(RequestContext requestContext) : Dsl
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
    private readonly DslFactory _fixture = new();
    
    [Fact]
    public async Task UserCanLogin()
    {
        var dsl = _fixture.GetDsl<ExampleServiceDsl>();
        
        await dsl.Login();
    }
}