using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Installation;

namespace Truss.Modeling.Domain.Tests.Unit;

#pragma warning disable CS8625
public sealed class DomainTestModule : IModule
{
    public void Define(IServiceCollection services, IConfiguration configuration)
    {
    }
}