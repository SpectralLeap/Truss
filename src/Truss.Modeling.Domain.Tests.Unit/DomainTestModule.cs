using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Installation;

namespace Truss.Modeling.Domain.Tests.Unit;

public sealed class DomainTestModule : Module
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }
}