using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore.Endpoints;

public abstract class EndpointModule : Module,
    IEndpointModule
{   
    /// <inheritdoc />
    /// <default>
    /// true
    /// </default>
    public virtual bool AutoMapMessagesAsEndpoints => true;

    public virtual void ConfigurePipeline(IApplicationBuilder app)
    {
    }

    public virtual void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
    }

    public virtual void Do(IWebHostBuilder builder)
    {
    }
}