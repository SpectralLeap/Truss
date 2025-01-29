using MediatR;
using Microsoft.Extensions.Logging;
using Truss.Monads.Results;

namespace Truss.Infrastructure.Serilog;

public sealed class SerilogLoggingBehavior<TRequest, TResponse>
    : ResultPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private string RequestName => typeof(TRequest).Name;
    private string ResponseName => typeof(TResponse).Name;

    private readonly ILogger<SerilogLoggingBehavior<TRequest, TResponse>> _logger;

    public SerilogLoggingBehavior(
        ILogger<SerilogLoggingBehavior<TRequest, TResponse>> logger
    )
    {
        _logger = logger;
    }

    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Request started: {RequestName} -> {ResponseName} ",
            RequestName,
            ResponseName
        );

        try
        {
            var result = await next().ConfigureAwait(false);

            await ParseResult(
                    result,
                    onSuccess: _ =>
                    {
                        _logger.LogDebug(
                            "{RequestName} succeeded"
                            , RequestName
                        );

                        return ValueTask.CompletedTask;
                    },
                    onFailure: details =>
                    {
                        _logger.LogDebug(
                            "{RequestName} failed: {Message}",
                            RequestName,
                            details.GetMessage()
                        );

                        return ValueTask.CompletedTask;
                    })
                .ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogCritical("{RequestName} failed with exception: {ExceptionMessage}\n{StackTrace}", RequestName, ex.Message, ex.StackTrace);

            throw;
        }
    }
}