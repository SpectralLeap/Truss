using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Application.Installation;

public interface ITrussModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration);
}