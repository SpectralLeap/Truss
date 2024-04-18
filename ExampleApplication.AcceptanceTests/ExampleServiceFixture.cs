using System.Net;
using ExampleApplication.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing;
using Truss.Testing.AspNetCore;
using Truss.Testing.Services;

namespace ExampleApplication.AcceptanceTests;

public class ExampleServiceFixture(HttpClient client) : Fixture
{
    [BaseServices]
    public static IServiceCollection ServiceProvider => new ServiceCollection()
        .AddWebServer<Program>()
    ;

    public async Task AssertHeartbeat()
    {
        Assert.NotNull(client);

        var request = new HttpRequestMessage(HttpMethod.Get, "heartbeat");
        
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        Assert.Equal("OK", await response.Content.ReadAsStringAsync());
    }
}