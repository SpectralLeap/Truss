using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.Installation;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Truss.AspNetCore.MessageToEndpointMapping;

public sealed class MessageToEndpointMappingInstallationStep
    : WebModuleInstallationStep
{
    private readonly ILogger<MessageToEndpointMappingInstallationStep> _logger;
    private readonly TrussServiceOptions _options;

    public MessageToEndpointMappingInstallationStep(
        ILogger<MessageToEndpointMappingInstallationStep> logger,
        TrussServiceOptions options
    )
    {
        _logger = logger;
        _options = options;
    }

    public override void Run(
        WebApplication app,
        ModuleManifest moduleManifest
    )
    {
        var endpointPrefix = GetEndpointPrefix(moduleManifest);

        if (moduleManifest.Module is WebModule { AutoMapMessagesAsEndpoints: true })
        {
            // Get all the types to scan excluding internal messages
            var types = moduleManifest.Types
                // Don't include internal messages
                .Where(t => t.GetCustomAttribute<InternalMessageAttribute>() is null)
                .ToArray();

            // Get commands that don't return a TResult
            var simpleCommands = types
                .Where(t => t.IsAssignableTo(typeof(ICommand)))
                .ToArray();

            // Get commands that do return a TResult
            var transactionalCommands = types
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>)))
                .ToArray();

            // Get queries
            var queries = types
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>)))
                .ToArray();

            // Map commands that don't return data
            foreach (var command in simpleCommands)
            {
                var route = $"{endpointPrefix}/{command.Name.Replace("Command", "")}";

                app.MapPost(route, BuildCommandHandler(command))
                    .WithTags("Commands")
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status401Unauthorized)
                    .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
                    .ProducesProblem(StatusCodes.Status500InternalServerError);
            }

            // Map commands returning data
            foreach (var command in transactionalCommands)
            {
                // Get the response type from the command's type argument
                var responseType = command.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
                    .GetGenericArguments()[0];

                var route = $"{endpointPrefix}/{command.Name.Replace("Command", "")}";

                app.MapPost(route, BuildCommandHandler(command, responseType))
                    .WithTags("Commands")
                    .Produces(StatusCodes.Status200OK, responseType)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    ;
            }

            // Map queries
            foreach (var query in queries)
            {
                // Get the response type from the query's type argument
                var responseType = query.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
                    .GetGenericArguments()[0];

                var route = $"{endpointPrefix}/{query.Name.Replace("Query", "")}";

                app.MapGet(route, BuildQueryHandler(query, responseType))
                    .WithTags("Queries")
                    .Produces(StatusCodes.Status200OK, responseType)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    ;
            }
        }
    }

    private string GetEndpointPrefix(
        ModuleManifest moduleManifest
    )
    {
        if (_options is not TrussWebServiceOptions webServiceConfiguration)
        {
            return "";
        }

        var prefix = webServiceConfiguration.ApiBasePath ?? "";

        prefix += webServiceConfiguration.UseModuleNameInApiPath
            ? $"/{moduleManifest.Name}"
            : "";

        return prefix;
    }

    private Delegate BuildCommandHandler(Type commandType)
    {
        var method = GetType()
            .GetMethod(nameof(HandleCommand), BindingFlags.Instance | BindingFlags.NonPublic)
            ?.MakeGenericMethod(commandType);

        if (method == null)
            throw new InvalidOperationException($"{nameof(HandleCommand)} method not found.");

        var funcType = typeof(Func<,,,>).MakeGenericType(
            commandType,
            typeof(ICommandBus),
            typeof(MessageToEndpointHandler),
            typeof(Task<IResult>)
        );

        return Delegate.CreateDelegate(funcType, this, method);
    }

    private Delegate BuildCommandHandler(Type commandType, Type responseType)
    {
        var method = GetType()
            .GetMethod(nameof(HandleTransaction), BindingFlags.Instance | BindingFlags.NonPublic)
            ?.MakeGenericMethod(commandType, responseType);

        if (method == null)
            throw new InvalidOperationException($"{nameof(HandleTransaction)} method not found.");

        var funcType = typeof(Func<,,,>).MakeGenericType(
            commandType,
            typeof(ICommandBus),
            typeof(MessageToEndpointHandler),
            typeof(Task<IResult>)
        );

        return Delegate.CreateDelegate(funcType, this, method);
    }

    private Delegate BuildQueryHandler(Type queryType, Type responseType)
    {
        var method = GetType()
            .GetMethod(nameof(HandleQuery), BindingFlags.Instance | BindingFlags.NonPublic)
            ?.MakeGenericMethod(queryType, responseType);

        if (method == null)
            throw new InvalidOperationException($"{nameof(HandleQuery)} method not found.");

        var funcType = typeof(Func<,,,>).MakeGenericType(
            queryType,
            typeof(IQueryBus),
            typeof(MessageToEndpointHandler),
            typeof(Task<IResult>)
        );

        return Delegate.CreateDelegate(funcType, this, method);
    }

    private async Task<IResult> HandleCommand<TCommand>(
        [FromBody] TCommand command,
        [FromServices] ICommandBus commandBus,
        [FromServices] MessageToEndpointHandler messageToEndpointHandler
    )
        where TCommand : ICommand
    {
        return await messageToEndpointHandler.SendMessage(
            command,
            // We want to send the command to the bus via arguments
            // so we don't build a closure around this method
            // which would use more memory
            async c => await commandBus.SendCommand(c)
        );
    }

    private async Task<IResult> HandleTransaction<TCommand, TResponse>(
        [FromBody] TCommand command,
        [FromServices] ICommandBus commandBus,
        [FromServices] MessageToEndpointHandler messageToEndpointHandler
    )
        where TCommand : ICommand<TResponse>
    {
        return await messageToEndpointHandler.SendMessage(
            command,
            // We want to send the command to the bus via arguments
            // so we don't build a closure around this method
            // which would use more memory
            async c => await commandBus.SendCommand(c)
        );
    }

    private async Task<IResult> HandleQuery<TQuery, TResponse>(
        [AsParameters] TQuery query,
        [FromServices] IQueryBus queryBus,
        [FromServices] MessageToEndpointHandler messageToEndpointHandler
    )
        where TQuery :IQuery<TResponse>
    {
        return await messageToEndpointHandler.SendMessage(
            query,
            // We want to send the query to the bus via arguments
            // so we don't build a closure around this method
            // which would use more memory
            async q => await queryBus.SendQuery(q)
        );
    }
}