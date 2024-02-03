namespace Truss.Results.Extensions.Functional;

public static class AsyncExtensions
{
     public static async Task<Result<TResult>> ThenAsync<TSuccess, TResult>(
          this Task<Result<TSuccess>> pendingResult,
          Func<TSuccess, TResult> map
      )
      {
          var result = await pendingResult.ConfigureAwait(false);
          
          if (result.Failed) return Result.Fail(result.FailureDetails);
                  
          try
          {
              return map(result.SuccessValue);
          }
          catch (Exception ex)
          {
              return Result.Fail(ex);
          }
      }
   
     public static async Task<Result<TResult>> ThenAsync<TSuccess, TResult>(
         this Result<TSuccess> result,
         Func<TSuccess, Task<Result<TResult>>> map
     )
     {
         if (result.Failed) return Result.Fail(result.FailureDetails);
                 
         try
         {
             return await map(result.SuccessValue).ConfigureAwait(false);
         }
         catch (Exception ex)
         {
             return Result.Fail(ex);
         }
     }
  
     public static async Task<Result<TResult>> ThenAsync<TSuccess, TResult>(
         this Task<Result<TSuccess>> pendingResult,
         Func<TSuccess, Task<Result<TResult>>> map
     )
     {
         var result = await pendingResult.ConfigureAwait(false);
 
         try
         {
             return await map(result.SuccessValue).ConfigureAwait(false);
         }
         catch (Exception ex)
         {
             return Result.Fail(ex);
         }
     }
      
     public static async Task<Result<TResult>> ThenAsync<TSuccess, TResult>(
         this Task<Result<TSuccess>> pendingResult,
         Func<TSuccess, Result<TResult>> map
     )
     {
          var result = await pendingResult.ConfigureAwait(false);
  
          try
          {
              return map(result.SuccessValue);
          }
          catch (Exception ex)
          {
              return Result.Fail(ex);
          }            
     }   
}

public static class ThenExtensions
{
    public static Result<TResult> Then<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, TResult> map
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            return map(result.SuccessValue);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
    
    public static Result<TResult> Then<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, Result<TResult>> map
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            return map(result.SuccessValue);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    public static Result<None> Then<TSuccess>(
        this Result<TSuccess> result,
        Action<TSuccess> map
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            map(result.SuccessValue);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
}

public static class BaseResultExtensions
{
    
    /// <summary>
    /// Perform an action on success or failure
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Resolve<TSuccess>(
        this Result<TSuccess> result,
        Action<TSuccess> forSuccess,
        Action<FailureDetails> forFailure
    )
    {
        result.ResolveInternal(forSuccess, forFailure);
    }

    /// <summary>
    /// Perform an action on success or failure
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static Task ResolveAsync<TSuccess>(
        this Result<TSuccess> result,
        Func<TSuccess, Task> forSuccess,
        Func<FailureDetails, Task> forFailure
    )
    {
        return result.ResolveInternalAsync(forSuccess, forFailure);
    }

    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TResult Resolve<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, TResult> forSuccess,
        Func<FailureDetails, TResult> forFailure
    )
    {
        return result.ResolveInternal(forSuccess, forFailure);
    }

    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <param name="result"></param>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<TResult> Resolve<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, Task<TResult>> forSuccess,
        Func<FailureDetails, Task<TResult>> forFailure
    )
    {
        return await result.ResolveInternalAsync(forSuccess, forFailure).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TSuccess? ResolveOrThrow<TSuccess>(this Result<TSuccess> result)
    {
        return result.ResolveInternal(
            s: s => result.SuccessValue,
            f: f =>
            {
                if (f.Exception is not null) throw f.Exception;
    
                throw new ResolutionException(f.FailureReasons);
            }
        );
    }

    /// <summary>
    /// Internal implementation of mapping
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <param name="fail"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static TResult MapCore<TSuccess, TResult, TMapped>(
        this Result<TSuccess> result,
        Func<TSuccess, TResult> mapping, 
        Func<FailureDetails, TResult> fail
    )
    {
        return result.ResolveInternal(mapping, fail);
    }

    /// <summary>
    /// Internal implementation of mapping
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <param name="fail"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> MapCoreAsync<TSuccess, TResult, TMapped>(
        this Result<TSuccess> result,
        Func<TSuccess, Task<TResult>> mapping, 
        Func<FailureDetails, Task<TResult>> fail
    )
    {
        return await result.ResolveInternalAsync(mapping, fail).ConfigureAwait(false);
    }
         
    private static void ResolveInternal<TSuccess>(
        this Result<TSuccess> result,
        Action<TSuccess> onSuccess,
        Action<FailureDetails> onFailure)
    {
        if (result.Failed)
        {
            onFailure(result.FailureDetails);
            return;
        }
    
        onSuccess(result.SuccessValue);
    }
        
    public static TResult ResolveInternal<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, TResult> s,
        Func<FailureDetails, TResult> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (result.Failed) return f(result.FailureDetails);
    
        try
        {
            return s(result.SuccessValue);
        }
        catch(Exception ex)
        {
            return f(FailureDetails.From(ex));
        }
    }
        
    public static async Task ResolveInternalAsync<TSuccess>(
        this Result<TSuccess> result,
        Func<TSuccess, Task> s,
        Func<FailureDetails, Task> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (result.Failed)
        {
            await f(result.FailureDetails).ConfigureAwait(false);
            return;
        }
    
        try
        {
            await s(result.SuccessValue).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            await f(FailureDetails.From(ex)).ConfigureAwait(false);
        }
    }
        
    public static async Task<TResult> ResolveInternalAsync<TSuccess, TResult>(
        this Result<TSuccess> result,
        Func<TSuccess, Task<TResult>> s,
        Func<FailureDetails, Task<TResult>> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (result.Failed) return await f(result.FailureDetails).ConfigureAwait(false);
    
        try
        {
            return await s(result.SuccessValue).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            return await f(FailureDetails.From(ex)).ConfigureAwait(false);
        }
    }
}

/// <summary>
/// Extensions for Result Calculations
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static Result<(T1, TAppend)> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Result<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);
        
        var nextResult = result.MapCore<T1, Result<TAppend>, TAppend>(
            mapping,
            Result.Fail);
    
        if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);
             
        return Result.Success((result.SuccessValue, nextResult.SuccessValue));
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static Result<(T1, TAppend)> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, TAppend> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(result.SuccessValue);
            
            return Result.Success((result.SuccessValue, nextResult));
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
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, Task<TAppend>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);
        
        try
        {
            var nextResult = await mapping(awaitedResult.SuccessValue).ConfigureAwait(false);
            
            return Result.Success((
                    awaitedResult.SuccessValue,
                    nextResult)
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
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, Task<Result<TAppend>>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);
        
        try
        {
            var nextResult = await mapping(awaitedResult.SuccessValue).ConfigureAwait(false);
        
            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);
            
            return Result.Success((
                    awaitedResult.SuccessValue,
                    nextResult.SuccessValue)
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
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Task<Result<TAppend>>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);
            
        try
        {
            var nextResult = await mapping(result.SuccessValue).ConfigureAwait(false);
            
            if (nextResult.Failed) return Result.Fail(nextResult.FailureDetails);
            
            return Result.Success((result.SuccessValue, nextResult.SuccessValue));
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
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Task<TAppend>> mapping)
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);
                    
        try
        {
            var nextResult = await mapping(result.SuccessValue).ConfigureAwait(false);
            
            return Result.Success((result.SuccessValue, nextResult));
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
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, TAppend> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);
                    
        try
        {
            var nextResult = mapping(awaitedResult.SuccessValue);
            
            return Result.Success((awaitedResult.SuccessValue, nextResult));
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
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, TOut>(
        this Result<(T1, T2)> result,
        Func<T1, T2, Result<TOut>> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);
        
        try
        {
            var nextResult = mapping(result.SuccessValue.Item1, result.SuccessValue.Item2);
        
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
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, TOut>(
        this Result<(T1, T2)> result,
        Func<T1, T2, TOut> mapping
    )
    {
        if (result.Failed) return Result.Fail(result.FailureDetails);
                
        try
        {
            var nextResult = mapping(result.SuccessValue.Item1, result.SuccessValue.Item2);
                
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
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Then<T1, T2, TOut>(
        this Task<Result<(T1, T2)>> result,
        Func<T1, T2, TOut> mapping
    )
    {
        var awaitedResult = await result.ConfigureAwait(false);
        
        if (awaitedResult.Failed) return Result.Fail(awaitedResult.FailureDetails);
                    
        try
        {
            var nextResult = mapping(awaitedResult.SuccessValue.Item1, awaitedResult.SuccessValue.Item2);
                    
            return Result.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
     
}