using ExampleApplication.WebApi.Services;
using MediatR;
using Truss.AspNetCore;

namespace ExampleApplication.WebApi;

public sealed class ExampleModule : EndpointModule
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(GreeterPipelineBehavior<,>)
        );
    }
}