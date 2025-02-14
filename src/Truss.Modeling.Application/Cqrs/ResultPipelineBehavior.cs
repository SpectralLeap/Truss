using System.Reflection;
using MediatR;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Cqrs;

/// <summary>
/// A pipeline behavior that allows handling results
/// </summary>
/// <typeparam name="TRequest">
/// A request type
/// </typeparam>
/// <typeparam name="TResult">
/// The result type
/// </typeparam>
public abstract class ResultPipelineBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
    where TResult : IResult
{
    /// <summary>
    /// Handles the request
    /// </summary>
    /// <param name="request">
    /// The request to handle
    /// </param>
    /// <param name="next">
    /// The next handler in the pipeline
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token
    /// </param>
    /// <returns></returns>
    public abstract Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken);

    /// <summary>
    /// Map a failed result to the current result type
    /// </summary>
    /// <param name="result">
    /// A failed result to copy from
    /// </param>
    /// <returns>
    /// A failed result
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If the result is malformed
    /// </exception>
    protected TResult Fail(
        IResult result
    )
    {
        if (result.Succeeded) throw new InvalidOperationException("Cannot fail a successful result");

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

        return failMethod.Invoke(null, [result.FailureDetails]) is TResult
            ? (TResult)failMethod.Invoke(null, [result.FailureDetails])!
            : throw new InvalidOperationException($"Could not construct a failure of type {typeof(TResult)}");
    }

    /// <summary>
    /// Create a new failed result
    /// </summary>
    /// <param name="failureType">
    /// The <see cref="FailureType"/> to fail for
    /// </param>
    /// <param name="reasons">
    /// The reasons for failure
    /// </param>
    /// <returns>
    /// A failed result
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If the result is malformed
    /// </exception>
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

    /// <summary>
    /// Parses a result and performs actions on success or failure
    /// </summary>
    /// <param name="result">
    /// The result to parse
    /// </param>
    /// <param name="onSuccess">
    /// The action to perform on success
    /// </param>
    /// <param name="onFailure">
    /// The action to perform on failure
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the result is malformed
    /// </exception>
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

    /// <summary>
    /// Parses a result and performs actions on success or failure
    /// </summary>
    /// <param name="result">
    /// The result to parse
    /// </param>
    /// <param name="onSuccess">
    /// The action to perform on success
    /// </param>
    /// <param name="onFailure">
    /// The action to perform on failure
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the result was malformed
    /// </exception>
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