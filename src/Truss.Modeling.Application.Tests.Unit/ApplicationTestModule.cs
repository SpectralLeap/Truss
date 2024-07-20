using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Application.Tests.Unit;

public sealed class ApplicationTestModule : IModule
{
    public void Define(IServiceCollection services, IConfiguration configuration)
    {
    }
}