using System.Reflection;
using MediatR;
using Truss.Monads.Results;

namespace Truss;

public abstract class ResultPipelineBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
    where TResult : IResult
{
    public abstract Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken);

    protected TResult Fail(
        FailureType failureType,
        params string[] reasons
    )
    {
        var innerType = typeof(TResult).GetGenericArguments()[0];

        var resultType = typeof(Result<>).MakeGenericType(innerType);

        var failMethod = resultType.GetMethod(
            "Fail",
            BindingFlags.Static | BindingFlags.Public,
            null,
            [typeof(FailureDetails)],
            null
        );

        if (failMethod == null)
        {
            throw new InvalidOperationException("Fail method not found on Result type.");
        }

        var failureDetails = FailureDetails.From(
            failureType,
            reasons
        );

        return failMethod.Invoke(null, [failureDetails]) is TResult
            ? (TResult)failMethod.Invoke(null, [failureDetails])!
            : throw new InvalidOperationException($"Could not construct a failure of type {typeof(TResult)}");
    }

    protected async ValueTask ParseResult(
        TResult result,
        Func<ValueTask> onSuccess,
        Func<FailureDetails, ValueTask> onFailure
    )
    {
        if (result.Succeeded)
        {
            await onSuccess().ConfigureAwait(false);

            return;
        }


        if (result.FailureDetails is null)
        {
            throw new InvalidOperationException($"FailureDetails are null for {typeof(TResult).Name}");
        }

        await onFailure(result.FailureDetails).ConfigureAwait(false);
    }

    protected async ValueTask ParseResult(
        TResult result,
        Func<object, ValueTask> onSuccess,
        Func<FailureDetails, ValueTask> onFailure
    )
    {
        if (result.Succeeded)
        {
            if (result.SuccessObject is null)
            {
                throw new InvalidOperationException($"SuccessObject is null for {typeof(TResult).Name}");
            }

            await onSuccess(result.SuccessObject).ConfigureAwait(false);
            return;
        }

        if (result.FailureDetails is null)
        {
            throw new InvalidOperationException($"FailureDetails are null for {typeof(TResult).Name}");
        }

        await onFailure(result.FailureDetails).ConfigureAwait(false);
    }
}