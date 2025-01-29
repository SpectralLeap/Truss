using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.Installation;
using Truss.Modeling.Installation;

namespace Truss.AspNetCore.Endpoints;

public interface IEndpointModule : IModule
{
    /// <summary>
    /// Configures the application pipeline for the module.
    /// This method allows the module to modify middleware, endpoints, or other application-specific behaviors.
   /// </summary>
    /// <param name="app">The application builder used to configure the pipeline.</param>
    public void ConfigurePipeline(
        IApplicationBuilder app
    );

    /// <summary>
    /// <p>
    /// If true, messages of the type
    /// <see cref="ICommand{TResult}"/>, <see cref="ICommand{TResult}"/>, and <see cref="IQuery{TResult}"/>
    /// are registered as minimal endpoints and wired up to their handlers.
    /// </p>
    /// <p>
    /// Registration of specific messages can be avoided by using the <see cref="InternalMessageAttribute"/>
    /// </p>
    /// </summary>
    public bool AutoMapMessagesAsEndpoints { get; }
    
    /// <summary>
    /// Maps the module's endpoints.
    /// </summary>
    /// <param name="endpoints"></param>
    public void MapEndpoints(
        IEndpointRouteBuilder endpoints
    );

    /// <summary>
    /// Builds the web host.
    /// </summary>
    /// <param name="builder"></param>
    public void Do(
        IWebHostBuilder builder
    );
}