using Truss.Modeling.Endpoints;

namespace Truss.Modeling.Module;

public interface IEndpointAggregator
{
    public IEndpointAggregator AddEndpoints<TEndpointAssemblyMarker>()
        where TEndpointAssemblyMarker : IEndpointAssemblyMarker;
}