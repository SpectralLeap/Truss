using Microsoft.Extensions.Logging;

namespace Truss.Results.Contextual;


public sealed class ResolutionStep<T1, T2>
{
    private readonly ILogger _logger;
    private readonly Result<(T1, T2)> _result;

    public ResolutionStep(ILogger logger, Result<(T1, T2)> result
    )
    {
        _logger = logger;
        _result = result;
    }
    
    public ResolutionStep<T1, T2> DoWith(
        Action<T1>? f1 = null,
        Action<T2>? f2 = null
    )
    {
        f1?.Invoke(_result.SuccessValue.Item1);
        f2?.Invoke(_result.SuccessValue.Item2);

        return this;
    }
}

public sealed class ResolutionStep<T>
{
    private readonly ILogger _logger;
    
    private readonly Result<T> _result;
    
    public ResolutionStep(ILogger logger, Result<T> result)
    {
        _logger = logger;
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }
    
    private ResolutionStep<TNext> Continuation<TNext>(Func<T, Result<TNext>> f)
    {
        if (_result.Failed) return new ResolutionStep<TNext>(_logger, Result.Fail(_result.FailureDetails));
        
        try
        {
            return new ResolutionStep<TNext>(_logger, f(_result.SuccessValue));
        }
        catch (Exception ex)
        {
            return new ResolutionStep<TNext>(_logger, Result.Fail(ex));
        }
    }
   
    private async Task<ResolutionStep<TNext>> ContinuationAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        if (_result.Failed) return new ResolutionStep<TNext>(_logger, Result.Fail(_result.FailureDetails));

        try
        {
            return new ResolutionStep<TNext>(_logger, await f(_result.SuccessValue));
        }
        catch (Exception ex)
        {
            return new ResolutionStep<TNext>(_logger, Result.Fail(ex));
        }
    }
    
    private ResolutionStep<T> SideEffect(Action<T> f)
    {
        if (_result.Failed) return this;
 
        try
        {
            f(_result.SuccessValue);
 
            return this;
        }
        catch (Exception ex)
        {
            return new ResolutionStep<T>(_logger, Result.Fail(ex));
        }
    }
     
    private async Task<ResolutionStep<T>> SideEffectAsync(Func<T, Task> f)
    {
        if (_result.Failed) return this;
 
        try
        {
            await f(_result.SuccessValue);
 
            return this;
        }
        catch (Exception ex)
        {
            return new ResolutionStep<T>(_logger, Result.Fail(ex));
        }
    }
     
    public ResolutionStep<TNext> Then<TNext>(Func<T, Result<TNext>> f)
    {
        return Continuation(f);
    }
    
    public ResolutionStep<TNext> Then<TNext>(Func<T, TNext> f)
    {
        return Continuation<TNext>(r => f(r));
    }
     
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        return await ContinuationAsync(f);
    }
    
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<TNext>> f)
    {
        return await ContinuationAsync<TNext>(async r => await f(r));
    }

    public ResolutionStep<T> Do<TOut>(Func<T, TOut> f)
    {
        return SideEffect(r => f(r));
    }
    
    public ResolutionStep<T> Do(Action<T> f)
    {
        return SideEffect(f);
                   
    }
    
    public ResolutionStep<T> Do(Action f)
    {
        return SideEffect(_ => f());
    }

    public ResolutionStep<T, TOther> And<TOther>(Func<T, TOther> f)
    {
        return new ResolutionStep<T, TOther>(
            _logger,
            Result.Success((_result.SuccessValue, f(_result.SuccessValue))));

    }
    
    public async Task<ResolutionStep<T>> DoAsync<TOut>(Func<T, Task<TOut>> f)
    {
        return await SideEffectAsync(f);
    }
    
    public async Task<ResolutionStep<T>> DoAsync<TOut>(Func<T, Task<Result<TOut>>> f)
    {
        return await SideEffectAsync(f);
    }
    
    
    public Result<T> Resolve()
    {
        return _result;
    }
}