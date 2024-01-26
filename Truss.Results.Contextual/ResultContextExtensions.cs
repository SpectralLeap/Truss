namespace Truss.Results.Contextual;

public static class ResultContextExtensions
{
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return step.Then(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return step.Then(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStep;
                    
        return step.Do(mapping);
    }
    
    public static async Task<ResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
                     
        return step.Do(mapping);
    }
     
    public static async Task<ResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStep;
            
        return await step.DoAsync(mapping);
    }
    
    public static async Task<ResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
                
        return await step.DoAsync(mapping);
    }
}