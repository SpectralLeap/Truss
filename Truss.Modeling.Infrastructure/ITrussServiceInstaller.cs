using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure;

public interface ITrussServiceInstaller
{
    public void InstallServices(IServiceCollection services);
}