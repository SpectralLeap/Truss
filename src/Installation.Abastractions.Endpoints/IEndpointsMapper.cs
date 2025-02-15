using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Installation.Abastractions.Endpoints;

public interface IEndpointsMapper
{
    public void MapEndpoints(
        IEndpointRouteBuilder endpointRouteBuilder
    );
}

public interface IApplicationPartsInstaller
{
    public void AddApplicationParts(
        IApplicationBuilder app,
        IServiceProvider serviceProvider
    );
}