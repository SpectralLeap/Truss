using Microsoft.Extensions.Logging;

namespace Truss.Results.Contextual;

public sealed class ResolutionStepRef<T>
{
    private readonly ILogger? _logger;
    
    private readonly Result<T> _result;
    
    public ResolutionStepRef(ILogger? logger, Result<T> result)
    {
        _logger = logger;
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    private ResolutionStepRef<TNext> Next<TNext>(Result<TNext> next)
    {
        return new ResolutionStepRef<TNext>(_logger, next);
    }
    
    private Result<TNext> Continuation<TNext>(Func<T, Result<TNext>> f)
    {
        if (_result.Failed) return Result.Fail(_result.FailureDetails!);
        
        try
        {
            return f(_result.SuccessValue);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
   
    private async Task<Result<TNext>> ContinuationAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        if (_result.Failed) return Result.Fail(_result.FailureDetails!);

        try
        {
            return await f(_result.SuccessValue).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
    
    private ResolutionStepRef<T> SideEffect(Action<T> f)
    {
        if (_result.Failed) Next(_result);
 
        try
        {
            f(_result.SuccessValue);
 
            return Next(_result);
        }
        catch (Exception ex)
        {
            return Next<T>(Result.Fail(ex));
        }
    }
     
    private async Task<ResolutionStepRef<T>> SideEffectAsync(Func<T, Task> f)
    {
        if (_result.Failed) return Next(_result);
 
        try
        {
            await f(_result.SuccessValue);
 
            return Next(_result);
        }
        catch (Exception ex)
        {
            return Next<T>(Result.Fail(ex));
        }
    }
     
    public ResolutionStepRef<TNext> Then<TNext>(Func<T, Result<TNext>> f)
    {
        return Next(Continuation(f));
    }
    
    public ResolutionStepRef<TNext> Then<TNext>(Func<T, TNext> f)
    {
        return Next(Continuation<TNext>(r => f(r)));
    }
     
    public async Task<ResolutionStepRef<TNext>> ThenAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        return Next(await ContinuationAsync(f));
    }
    
    public async Task<ResolutionStepRef<TNext>> ThenAsync<TNext>(Func<T, Task<TNext>> f)
    {
        return Next(await ContinuationAsync<TNext>(async r => await f(r)));
    }

    public ResolutionStepRef<T> Do<TOut>(Func<T, TOut> f)
    {
        var result = Continuation<TOut>(r => f(r));

        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
        
        return Next(_result);
    }
    
    public ResolutionStepRef<T> Do(Action<T> f)
    {
        return SideEffect(f);
                   
    }
    
    public ResolutionStepRef<T> Do(Action f)
    {
        return SideEffect(_ => f());
    }
    
    public ResolutionStepRef<T> Do<TOut>(Func<T, Result<TOut>> f)
    {
        var result = Continuation(f);
        
        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
         
        return Next(_result);
    }
 
    public ResolutionStepRef<T, TOther> And<TOther>(Func<T, TOther> f)
    {
        var next = Continuation<TOther>(r => f(r));

        if (next.Failed) return new ResolutionStepRef<T, TOther>(_logger, Result.Fail(next.FailureDetails));
        
        return new ResolutionStepRef<T, TOther>(
            _logger,
            (_result.SuccessValue, next.SuccessValue)
        );

    }
    
    public async Task<ResolutionStepRef<T>> DoAsync<TOut>(Func<T, Task<TOut>> f)
    {
        return await SideEffectAsync(f);
    }
    
    public async Task<ResolutionStepRef<T>> DoAsync<TOut>(Func<T, Task<Result<TOut>>> f)
    {
        return await SideEffectAsync(f);
    }
    
    
    public Result<T> Resolve()
    {
        return _result;
    }
}

public sealed class ResolutionStepRef<T1, T2>
{
    private readonly ILogger? _logger;
    private readonly Result<(T1, T2)> _result;

    public ResolutionStepRef(ILogger? logger, Result<(T1, T2)> result
    )
    {
        _logger = logger;
        _result = result;
    }

    public ResolutionStepRef<T1, T2> DoWith(
        Action<T1>? f1 = null,
        Action<T2>? f2 = null
    )
    {
        f1?.Invoke(_result.SuccessValue.Item1);
        f2?.Invoke(_result.SuccessValue.Item2);

        return new ResolutionStepRef<T1, T2>(_logger, _result);
    }

    public static implicit operator (T1, T2)(ResolutionStepRef<T1, T2> resolutionStep) =>
        (resolutionStep._result.SuccessValue.Item1, resolutionStep._result.SuccessValue.Item2);
}