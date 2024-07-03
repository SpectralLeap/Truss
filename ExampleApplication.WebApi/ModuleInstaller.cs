using ExampleApplication.WebApi.Services;
using MediatR;
using Truss.Modeling.Module;

public sealed class ModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(GreeterPipelineBehavior<,>));
    }
}

public sealed class Endpoints : IEndpointInstaller
{
    public void InstallEndpoints(IEndpointAggregator endpointAggregator)
    {
    }
}