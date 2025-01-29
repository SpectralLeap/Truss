using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore;

/// <summary>
/// A module that will be installed in a web hosted context (i.e. web app)
/// </summary>
public abstract class WebModule : Module
{   
    /// <summary>
    /// If true, the module will automatically map messages, such as commands and queries, as endpoints.
    /// </summary>
    public virtual bool AutoMapMessagesAsEndpoints => false;

    /// <summary>
    /// Gives the module access to configure the application builder
    /// </summary>
    /// <param name="builder">
    /// The application builder
    /// </param>
    public virtual void ConfigureWebApplicationBuilder(WebApplicationBuilder builder)
    {
    }

    /// <summary>
    /// Gives the module access to configure the application
    /// </summary>
    /// <param name="app">
    /// The application
    /// </param>
    public virtual void ConfigureApplication(WebApplication app)
    {
    }

    /// <summary>
    /// Gives the module access to configure the endpoint route builder
    /// </summary>
    /// <param name="builder">
    /// The endpoint route builder
    /// </param>
    public virtual void MapEndpoints(IEndpointRouteBuilder builder)
    {
    }
}