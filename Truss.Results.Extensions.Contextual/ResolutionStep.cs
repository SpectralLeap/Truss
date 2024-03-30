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

    private Func<Result<TOut>> UnlessFailedDo<TOut>(Func<Result<TOut>> f)
    {
        var failedResult = _results.FirstOrDefault(result => result.Failed);
        
        if (failedResult is not null) return () => Result.Fail(failedResult.FailureDetails!);
        
        return f;
    }

    private Result<TOut> TrappingExceptions<TOut>(Func<Result<TOut>> f)
    {
         try
         {
             return f();
         }
         catch (Exception ex)
         {
             return Result.Fail(ex);
         }       
    }
    
    protected Result<TOut> Continuation<TOut>(Func<object[], Result<TOut>> execute)
    {
        var f = () => 
            (Result<TOut>) execute.DynamicInvoke(
                _results.Select(result => result.SuccessObject)
            );
        
        return TrappingExceptions(UnlessFailedDo(f));
    }
    
    protected async Task<Result<TNext>> ContinuationAsync<TNext>(Func<object[], Task<Result<TNext>>> execute)
    {
        var f = () => (Task<Result<TNext>>)execute
            .DynamicInvoke(_results.Select(result => result.SuccessObject));
        
        return TrappingExceptions(UnlessFailedDo<TNext>(() => f().Wait()));
    }
    
    protected ResolutionStep<T> SideEffect<T>(Action<object[]> f)
    {
        return TrappingExceptions(UnlessFailedDo(() =>
        {
            f(_results);
        }));
    }
     
    protected async Task<ResolutionStep<T>> SideEffectAsync<T>(Func<T, Task> f)
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
     
    protected ResolutionStep<TNext> Next<TNext>(Result<TNext> next)
    {
        return new ResolutionStep<TNext>(_logger, next);
    }
}

public sealed class ResolutionStep<T> : ResolutionStep
{
    private readonly ILogger? _logger;
    
    private readonly Result<T> _result;
    
    public ResolutionStep(ILogger? logger, Result<T> result) : base(logger, result)
    {
        _logger = logger;
    }
    
    
    public ResolutionStep<TNext> Then<TNext>(Func<T, Result<TNext>> f)
    {
        return Next(Continuation(args => f((T)args[0])));
    }
    
    public ResolutionStep<TNext> Then<TNext>(Func<T, TNext> f)
    {
        return Next(Continuation(r => f(r)));
    }
     
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<Result<TNext>>> f)
    {
        return Next(await ContinuationAsync(f));
    }
    
    public async Task<ResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<TNext>> f)
    {
        return Next(await ContinuationAsync<TNext>(async r => await f((T)r[0])));
    }

    public ResolutionStep<T> Perform<TOut>(Func<T, TOut> f)
    {
        var result = Continuation<TOut>(r => f((T)r[0]));

        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
        
        return Next(_result);
    }
    
    public ResolutionStep<T> Perform(Action<T> f)
    {
        return SideEffect(f);
    }
    
    public ResolutionStep<T> Perform(Action f)
    {
        return SideEffect<T>(_ => f());
    }
    
    public ResolutionStep<T> Perform<TOut>(Func<T, Result<TOut>> f)
    {
        var result = Continuation(f);
        
        if (result.Failed) return Next<T>(Result.Fail(result.FailureDetails));
         
        return Next(_result);
    }
 
    public ResolutionStep<T, TOther> And<TOther>(Func<T, TOther> f)
    {
        var next = Continuation<TOther>(r => f((T)r[0]));

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