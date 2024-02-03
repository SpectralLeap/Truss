namespace Truss.Results.Extensions.Functional;

/// <summary>
/// Result extension for arity of 3
/// </summary>
public static class ResultExtensions3
{
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, T3, TAppend)> And<T1, T2, T3, TAppend>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, Result<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            );

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);
            
            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
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
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, T3, TAppend)> And<T1, T2, T3, TAppend>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, TAppend> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            );

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
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
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, TAppend)>> And<T1, T2, T3, TAppend>(
        this Task<Result<(T1, T2, T3)>> result,
        Func<T1, T2, T3, Task<TAppend>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3
            ).ConfigureAwait(false);

            return Result.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    awaitedResult.SuccessValue.Item3,
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
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, TAppend)>> And<T1, T2, T3, TAppend>(
        this Task<Result<(T1, T2, T3)>> result,
        Func<T1, T2, T3, Task<Result<TAppend>>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);

            return Result.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    awaitedResult.SuccessValue.Item3,
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
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, TAppend)>> And<T1, T2, T3, TAppend>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, Task<Result<TAppend>>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
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
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, TAppend)>> And<T1, T2, T3, TAppend>(
        this Result<(T1, T2, T3)> result,
        Func<T1, T2, T3, Task<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3
            ).ConfigureAwait(false);

            return Result.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
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
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, T4, TOut>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, Result<TOut>> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3, result.SuccessValue.Item4
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
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, T4, TOut>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, TOut> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3, result.SuccessValue.Item4
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
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Then<T1, T2, T3, T4, TOut>(
        this Task<Result<(T1, T2, T3, T4)>> result,
        Func<T1, T2, T3, T4, TOut> mapping
    )
    {
        var awaitedResult = await result.ConfigureAwait(false);

        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3, 
                awaitedResult.SuccessValue.Item4
            );

            return Result.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
}