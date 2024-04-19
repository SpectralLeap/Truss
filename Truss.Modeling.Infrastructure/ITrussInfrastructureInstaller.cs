using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure;

public interface ITrussInfrastructureInstaller
{
    public void Install(IServiceCollection services, TrussConfig config);
}