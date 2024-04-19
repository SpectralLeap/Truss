using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure.Installation;

public interface IInfrastructure
{
    public void Define(
        IServiceCollection services,
        TrussServiceConfiguration serviceConfiguration,
        IConfiguration configuration
    );
}