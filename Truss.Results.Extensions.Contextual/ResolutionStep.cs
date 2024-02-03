using Microsoft.Extensions.Logging;

namespace Truss.Results.Extensions.Contextual;

public abstract class ResolutionStep
{
    private readonly ILogger? _logger;
    private readonly IResult[] _results;

    protected ResolutionStep(ILogger? logger, params IResult[] results)
    {
        _logger = logger;
        _results = results;
    }
    
    protected Result<TNext> Continuation<T, TNext>(Func<IResult[], T> select, Func<T, Result<TNext>> execute)
    {
        var failedResult = _results.FirstOrDefault(result => result.Failed);
        if (failedResult is not null) return Result.Fail(failedResult.FailureDetails!);
            
        try
        {
            return execute(select(_results));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }
     
}

public sealed class ResolutionStep<T> : ResolutionStep
{
    private readonly ILogger? _logger;
    
    private readonly Result<T> _result;
    
    public ResolutionStep(ILogger? logger, Result<T> result) : base(logger, result)
    {
        _logger = logger;
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    private ResolutionStep<TNext> Next<TNext>(Result<TNext> next)
    {
        return new ResolutionStep<TNext>(_logger, next);
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
    
    private ResolutionStep<T> SideEffect(Action<T> f)
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
     
    private async Task<ResolutionStep<T>> SideEffectAsync(Func<T, Task> f)
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
     
    public ResolutionStep<TNext> Then<TNext>(Func<T, Result<TNext>> f)
    {
        return Next(Continuation(f));
    }
    
    public ResolutionStep<TNext> Then<TNext>(Func<T, TNext> f)
    {
        return Next(Continuation<TNext>(r => f(r)));
    }
     
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        return Next(await ContinuationAsync(f));
    }
    
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<TNext>> f)
    {
        return Next(await ContinuationAsync<TNext>(async r => await f(r)));
    }

    public ResolutionStep<T> Perform<TOut>(Func<T, TOut> f)
    {
        var result = Continuation<TOut>(r => f(r));

        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
        
        return Next(_result);
    }
    
    public ResolutionStep<T> Perform(Action<T> f)
    {
        return SideEffect(f);
                   
    }
    
    public ResolutionStep<T> Perform(Action f)
    {
        return SideEffect(_ => f());
    }
    
    public ResolutionStep<T> Perform<TOut>(Func<T, Result<TOut>> f)
    {
        var result = Continuation(f);
        
        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
         
        return Next(_result);
    }
 
    public ResolutionStep<T, TOther> And<TOther>(Func<T, TOther> f)
    {
        var next = Continuation<TOther>(r => f(r));

        if (next.Failed) return new ResolutionStep<T, TOther>(_logger, Result.Fail(next.FailureDetails));
        
        return new ResolutionStep<T, TOther>(
            _logger,
            (_result.SuccessValue, next.SuccessValue)
        );

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

public sealed class ResolutionStep<T1, T2>
{
    private readonly ILogger? _logger;
    private readonly Result<(T1, T2)> _result;

    public ResolutionStep(ILogger? logger, Result<(T1, T2)> result
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

        return new ResolutionStep<T1, T2>(_logger, _result);
    }

    public static implicit operator (T1, T2)(ResolutionStep<T1, T2> resolutionStep) =>
        (resolutionStep._result.SuccessValue.Item1, resolutionStep._result.SuccessValue.Item2);
}