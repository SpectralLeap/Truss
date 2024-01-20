namespace Truss.Results.Functional;

public sealed class ResolutionContext
{
    public ResolutionFactory<T> Using<T>(Result<T> result)
    {
        return new ResolutionFactory<T>(result);
    }
}

public sealed class ResolutionFactory<T>
{
    private readonly Result<T> _result;

    public ResolutionFactory(Result<T> result)
    {
        _result = result;
    }
}

/// <summary>
/// Extensions to add mapping functionality to results
/// </summary>
public static class ResultMappingExtensions
{
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Task<Result<TMapped>>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<TSuccess, Result<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TMapped>(
        this Task<Result> result,
        Func<Unit, Result<TMapped>> mapping)
    {
        return (await result.ConfigureAwait(false)).MapCoreAsync<Result, Result<TMapped>, TMapped>(
            mapping,
            Result<TMapped>.Fail);
    }
       
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result> Then(
        this Task<Result> result,
        Func<Unit, Task<Result>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result, Unit>(
            mapping,
            f => Task.FromResult(Result.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result> Then<TSuccess>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Task<Result>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result, Unit>(
            mapping,
            f => Task.FromResult(Result.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result> Then<TSuccess>(
        this Task<Result<TSuccess>> result,
        Action<TSuccess> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result, Unit>(
            v =>
            {
                mapping(v);
                return Task.FromResult(Result.Success());
            },
            
            f => Task.FromResult(Result.Fail(f))).ConfigureAwait(false);
    }
     
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result> Then(
        this Task<Result> result,
        Func<Unit, Result> mapping)
    {
        return (await result.ConfigureAwait(false)).MapCore<Result, Unit>(
            mapping,
            Result.Fail);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TMapped>(
        this Task<Result> result,
        Func<Unit, Task<Result<TMapped>>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result<TMapped>, Unit>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TMapped>(
        this Task<Result> result,
        Func<Task<Result<TMapped>>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result<TMapped>, Unit>(
            _ => mapping(),
            f => Task.FromResult(Result<TMapped>.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, TMapped> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result<TMapped>, Unit>(
            v => Task.FromResult(Result<TMapped>.Success(mapping(v))),
            f => Task.FromResult(Result<TMapped>.Fail(f))).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Then<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Result<TMapped>> mapping)
    {
        return await (await result.ConfigureAwait(false)).MapCoreAsync<Result<TMapped>, Unit>(
            v => Task.FromResult(mapping(v)),
            f => Task.FromResult(Result<TMapped>.Fail(f))).ConfigureAwait(false);
    }

    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <returns></returns>
    public static async Task<TMapped> Resolve<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, TMapped> forSuccess,
        Func<FailureDetails, TMapped> forFailure
    )
    {
        return await (await result.ConfigureAwait(false)).Resolve(
            success => Task.FromResult(forSuccess(success)),
            failure => Task.FromResult(forFailure(failure))
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <returns></returns>
    public static async Task<TMapped> Resolve<TMapped>(
        this Task<Result> result,
        Func<Unit, TMapped> forSuccess,
        Func<FailureDetails, TMapped> forFailure
    )
    {
        return await (await result.ConfigureAwait(false)).Resolve(
            success => Task.FromResult(forSuccess(success)),
            failure => Task.FromResult(forFailure(failure))
        ).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <returns></returns>
    public static async Task<TMapped> Resolve<TSuccess, TMapped>(
        this Task<Result<Task<TSuccess>>> result,
        Func<TSuccess, TMapped> forSuccess,
        Func<FailureDetails, TMapped> forFailure
    )
    {
        return await (await result.ConfigureAwait(false)).Resolve(
            async success => forSuccess(await success),
            failure => Task.FromResult(forFailure(failure))
        );
    }
}