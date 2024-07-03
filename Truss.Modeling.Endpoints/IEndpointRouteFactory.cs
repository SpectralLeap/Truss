namespace Truss.Modeling.Endpoints;

public interface IEndpointRouteFactory
{
    public string[] GetRoute(IRoutable routable);
    public string[] GetRoute<TRoutable>()
        where TRoutable : IRoutable;
}