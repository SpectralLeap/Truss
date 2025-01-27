using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.Queries;

namespace Truss.AspNetCore;

public sealed class EndpointInstallationStep 
    : WebInstallationStep
{
    private readonly ILogger<EndpointInstallationStep> _logger;

    public EndpointInstallationStep(
        ILogger<EndpointInstallationStep> logger
    )
    {
        _logger = logger;
    }

    public override void Run(
        WebApplication app,
        ModuleManifest moduleManifest
    )
    {
        if (moduleManifest.module is IEndpointModule { AutoMapMessagesAsEndpoints: true })
        {
            var commandTypes = moduleManifest.Types
                .Where(t =>
                {
                    return t
                        .GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Any(i => i.GetGenericTypeDefinition() == typeof(ICommand));
                })
                .ToArray();
            
            var transactionTypes = moduleManifest.Types
                .Where(t =>
                {
                    return t
                        .GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Any(i => i.GetGenericTypeDefinition() == typeof(ICommand<>));
                })
                .ToArray();
             
            var queryTypes = moduleManifest.Types
                .Where(t =>
                {
                    return t
                        .GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Any(i => i.GetGenericTypeDefinition() == typeof(IQuery<>));
                })
                .ToArray();
             
            foreach (var messageType in commandTypes)
            {
                _logger.LogDebug("Mapping endpoint for command {MessageType}", messageType.Name);
            }
            
            foreach (var messageType in transactionTypes)
            {
                _logger.LogDebug("Mapping endpoint for transaction {MessageType}", messageType.Name);
            }
            
            foreach (var messageType in queryTypes)
            {
                _logger.LogDebug("Mapping endpoint for query {MessageType}", messageType.Name);
            }
        }
    }
}