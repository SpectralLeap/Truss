using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Application;

public interface ITrussModuleInstaller
{
    public void Install(IServiceCollection services);
}