using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure.Installation;

public interface ITrussInfrastructureInstaller
{
    public void Install(IServiceCollection services, TrussServiceConfiguration serviceConfiguration, IConfiguration configuration);
}