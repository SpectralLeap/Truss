namespace Truss.Results.Extensions.Functional;

/// <summary>
/// Result extension for arity of 2
/// </summary>
public static class ResultExtensions2
{
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, TAppend)> And<T1, T2, TAppend>(
        this Result<(T1, T2)> result,
        Func<T1, T2, Result<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2
            );

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);
            
            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                nextResult.SuccessValue
            ));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, TAppend)> And<T1, T2, TAppend>(
        this Result<(T1, T2)> result,
        Func<T1, T2, TAppend> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2
            );

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                nextResult
                ));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, TAppend)>> And<T1, T2, TAppend>(
        this Task<Result<(T1, T2)>> result,
        Func<T1, T2, Task<TAppend>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2
            ).ConfigureAwait(false);

            return Result.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    nextResult
                    )
            );
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, TAppend)>> And<T1, T2, TAppend>(
        this Task<Result<(T1, T2)>> result,
        Func<T1, T2, Task<Result<TAppend>>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);

            return Result.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    nextResult.SuccessValue
                    )
            );
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, TAppend)>> And<T1, T2, TAppend>(
        this Result<(T1, T2)> result,
        Func<T1, T2, Task<Result<TAppend>>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                nextResult.SuccessValue
            ));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, TAppend)>> And<T1, T2, TAppend>(
        this Result<(T1, T2)> result,
        Func<T1, T2, Task<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2
            ).ConfigureAwait(false);

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                nextResult
                ));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, TOut>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, Result<TOut>> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            );

            return nextResult;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, TOut>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, TOut> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            );

            return Result.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Then<T1, T2, T3, TOut>(
        this Task<Result<(T1, T2, T3)>> result,
        Func<T1, T2, T3, TOut> mapping
    )
    {
        var awaitedResult = await result.ConfigureAwait(false);

        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3
            );

            return Result.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
}