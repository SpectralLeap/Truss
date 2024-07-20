using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Application.Installation;

public interface IModule
{
    public void Define(
        IServiceCollection services,
        IConfiguration configuration
    );
}