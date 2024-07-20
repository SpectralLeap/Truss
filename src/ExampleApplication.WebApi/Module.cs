using ExampleApplication.WebApi.Services;
using MediatR;
using Truss.Modeling.Application.Installation;

public sealed class Module : IModule
{
    public void Define(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(GreeterPipelineBehavior<,>));
    }
}