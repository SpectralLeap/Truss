namespace Truss.Results.Contextual;


public readonly struct AsyncResolutionStep<T>
{
    private readonly Result<T> _result;

    public AsyncResolutionStep(Result<T> result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    public Result<T> Resolve()
    {
        return _result;
    }
}

public readonly struct ResolutionStep<T>
{
    private readonly Result<T> _result;

    public ResolutionStep(Result<T> result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    public ResolutionStep<TNext> Then<TNext>(Func<T, Result<TNext>> f)
    {
        if (_result.Failed) return new ResolutionStep<TNext>(Result.Fail(_result.FailureDetails));

        try
        {
            return new ResolutionStep<TNext>(f(_result.SuccessValue));
        }
        catch(Exception ex)
        {
            return new ResolutionStep<TNext>(Result.Fail(ex));
        }
    }
    
    public ResolutionStep<TNext> Then<TNext>(Func<T, TNext> f)
    {
        if (_result.Failed) return new ResolutionStep<TNext>(Result.Fail(_result.FailureDetails));
 
        try
        {
            return new ResolutionStep<TNext>(f(_result.SuccessValue));
        }
        catch(Exception ex)
        {
            return new ResolutionStep<TNext>(Result.Fail(ex));
        }           
    }

        
    public async Task<AsyncResolutionStep<TNext>> ThenAsync<TNext>(Func<T, Task<TNext>> f)
    {
        if (_result.Failed) return new AsyncResolutionStep<TNext>(Result.Fail(_result.FailureDetails));
 
        try
        {
            var nextResult = await f(_result.SuccessValue).ConfigureAwait(false);
            
            return new AsyncResolutionStep<TNext>(nextResult);
        }
        catch(Exception ex)
        {
            return new AsyncResolutionStep<TNext>(Result.Fail(ex));
        }           
    }

    public ResolutionStep<T> Do<TOut>(Func<T, TOut> f)
    {
        if (_result.Failed) return this;
  
        try
        {
            f(_result.SuccessValue);
             
            return this;
        }
        catch(Exception ex)
        {
            return new ResolutionStep<T>(Result.Fail(ex));
        }

    }
    
    public ResolutionStep<T> Do(Action<T> f)
    {
        if (_result.Failed) return this;
   
        try
        {
            f(_result.SuccessValue);
              
            return this;
        }
        catch(Exception ex)
        {
            return new ResolutionStep<T>(Result.Fail(ex));
        }           
    }

    public ResolutionStep<T> Do(Action f)
    {
        if (_result.Failed) return this;
       
        try
        {
            f();
                  
            return this;
        }
        catch(Exception ex)
        {
            return new ResolutionStep<T>(Result.Fail(ex));
        }           
    }
    
    public async Task<ResolutionStep<T>> DoAsync<TOut>(Func<T, Task<TOut>> f)
    {
        if (_result.Failed) return this;
           
        try
        {
            await f(_result.SuccessValue).ConfigureAwait(false);
                      
            return this;
        }
        catch(Exception ex)
        {
            return new ResolutionStep<T>(Result.Fail(ex));
        }           
    }

    public async Task<ResolutionStep<T>> DoAsync<TOut>(Func<T, Task> f)
    {
         if (_result.Failed) return this;
            
         try
         {
             await f(_result.SuccessValue).ConfigureAwait(false);
                       
             return this;
         }
         catch(Exception ex)
         {
             return new ResolutionStep<T>(Result.Fail(ex));
         }                  
    }   
    
    public Result<T> Resolve()
    {
        return _result;
    }
}

public sealed class ResolutionContext
{
    public ResolutionStep<T> Start<T>(Result<T> result)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));

        return new ResolutionStep<T>(result);
    }
}