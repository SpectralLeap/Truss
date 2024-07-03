using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Module;

public interface IModuleInstaller
{
    public void Install(
        IServiceCollection services,
        IConfiguration configuration
    );
}