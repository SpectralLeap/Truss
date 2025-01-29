using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Truss.Monads.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Truss.AspNetCore.Endpoints;

internal sealed class EndpointHandler
{
    private readonly ILogger<EndpointHandler> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public EndpointHandler(
        ILogger<EndpointHandler> logger,
        IHttpContextAccessor contextAccessor
    )
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    public async Task<IResult> SendMessage<TRequest, TResponse>(
        TRequest request,
        Func<TRequest, Task<Result<TResponse>>> call
    )
    {
        if (request is null)
        {
            _logger.LogWarning("No request was provided");
            throw new ArgumentNullException(nameof(request));
        }

        if (call is null)
        {
            _logger.LogWarning("No action was provided");
            throw new ArgumentNullException(nameof(call));
        }

        var context = _contextAccessor.HttpContext;

        if (context is null)
        {
            _logger.LogCritical("The HTTP context was not available");

            return Results.Json(
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Internal server error",
                }
            );
        }

        var result = await call(request);

        if (result.Succeeded) return Results.Ok(result.SuccessValue);

        return MapFailureToProblem(result);
    }

    private static IResult MapFailureToProblem<TResponse>(Result<TResponse> result)
    {
        return result.FailureDetails.FailureType switch
        {
            FailureType.Failed => Results.Json(
                new ProblemDetails
                {
                    Title = "Unprocessable Entity",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = result.FailureMessage,
                }
            ),
            FailureType.Error => Results.Json(
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = result.FailureMessage,
                }
            ),
            FailureType.Unauthenticated => Results.Json(
                new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = result.FailureMessage
                }
            ),
            FailureType.Unauthorized => Results.Json(
                new ProblemDetails
                {
                    Title = "Forbidden",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = result.FailureMessage
                }
            ),
            FailureType.Validation => Results.Json(
                new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = result.FailureMessage,
                }
            ),
            FailureType.Cancelled => Results.Json(
                new ProblemDetails
                {
                    Title = "Cancelled",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = result.FailureMessage
                }
            ),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}