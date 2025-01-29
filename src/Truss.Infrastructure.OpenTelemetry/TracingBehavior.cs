using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Truss.Modeling.Application.Cqrs;
using Truss.Monads.Results;

namespace Truss.Infrastructure.OpenTelemetry;

public sealed class TracingBehavior<TRequest, TResponse>
    : ResultPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly ILogger<TracingBehavior<TRequest, TResponse>> _logger;
    private readonly ActivitySource _activitySource;

    public TracingBehavior(
        ILogger<TracingBehavior<TRequest, TResponse>> logger,
        ActivitySource activitySource
    )
    {
        _logger = logger;
        _activitySource = activitySource;
    }

    public override async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        using var activity = _activitySource.StartActivity(TracingInfo.ActivitySourceName);

        if (activity is null) return await next();

        _logger.LogDebug("Starting telemetry for {RequestType}", typeof(TRequest).Name);

        activity.SetTag("request.type", typeof(TRequest).FullName);
        activity.SetTag("response.type", typeof(TResponse).FullName);

        try
        {
            var response = await next();

            await ParseResult(
                response,
                onSuccess: () =>
                {
                    activity.SetTag("response.status", "success");
                    return ValueTask.CompletedTask;
                },
                onFailure: details =>
                {
                    activity.SetTag("response.status", "failure");
                    activity.SetTag("response.failure", details.GetMessage());
                    return ValueTask.CompletedTask;
                });

            return response;
        }
        catch (Exception ex)
        {
            activity.AddException(ex);

            activity.SetStatus(ActivityStatusCode.Error, ex.Message);

            throw;
        }
    }
}