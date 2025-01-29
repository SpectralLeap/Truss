using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore.Endpoints;

public abstract class EndpointModule : Module
{   
    /// <summary>
    /// If true, the module will automatically map messages, such as commands and queries, as endpoints.
    /// </summary>
    public virtual bool AutoMapMessagesAsEndpoints => true;

    /// <summary>
    /// Gives the module access to configure the application pipeline.
    /// </summary>
    /// <param name="app"></param>
    public virtual void ConfigurePipeline(IApplicationBuilder app)
    {
    }

    /// <summary>
    /// Gives the module access to configure routing.
    /// </summary>
    /// <param name="endpoints"></param>
    public virtual void ConfigureRouting(IEndpointRouteBuilder endpoints)
    {
    }
}