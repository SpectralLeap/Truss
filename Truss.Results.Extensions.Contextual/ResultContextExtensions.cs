namespace Truss.Results.Extensions.Contextual;
public static class ResultContextRefExtensions
{
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStepRef;
        
        return step.Then(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
        
        return step.Then(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ResolutionStep<T>> PerformAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStepRef;
                    
        return step.Perform(mapping);
    }
    
    public static async Task<ResolutionStep<T>> PerformAsync<T>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Action<T> mapping
    )
    {
        var step = await asyncResolutionStepRef;
                     
        return step.Perform(mapping);
    }
     
    public static async Task<ResolutionStep<T>> PerformAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
                     
        return step.Perform(mapping);
    }
     
    public static async Task<ResolutionStep<T>> PerformAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
            
        return await step.DoAsync(mapping);
    }
    
    public static async Task<ResolutionStep<T>> PerformAsync<T, TNext>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStepRef;
                
        return await step.DoAsync(mapping);
    }
    
    public static async Task<Result<T>> Resolve<T>(
        this Task<ResolutionStep<T>> asyncResolutionStepRef
    )
    {
        var step = await asyncResolutionStepRef;

        return step.Resolve();
    }
}
public static class ResultContextExtensions
{
    public static async Task<ValueResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return step.Then(mapping);
    }
    
    public static async Task<ValueResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return step.Then(mapping);
    }
    
    public static async Task<ValueResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ValueResolutionStep<TNext>> ThenAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStep;
        
        return await step.ThenAsync(mapping);
    }
    
    public static async Task<ValueResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, TNext> mapping
    )
    {
        var step = await asyncResolutionStep;
                    
        return step.Do(mapping);
    }
    
    public static async Task<ValueResolutionStep<T>> DoAsync<T>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Action<T> mapping
    )
    {
        var step = await asyncResolutionStep;
                     
        return step.Do(mapping);
    }
     
    public static async Task<ValueResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Result<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
                     
        return step.Do(mapping);
    }
     
    public static async Task<ValueResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<Result<TNext>>> mapping
    )
    {
        var step = await asyncResolutionStep;
            
        return await step.DoAsync(mapping);
    }
    
    public static async Task<ValueResolutionStep<T>> DoAsync<T, TNext>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep,
        Func<T, Task<TNext>> mapping
    )
    {
        var step = await asyncResolutionStep;
                
        return await step.DoAsync(mapping);
    }
    
    public static async Task<Result<T>> Resolve<T>(
        this Task<ValueResolutionStep<T>> asyncResolutionStep
    )
    {
        var step = await asyncResolutionStep;

        return step.Resolve();
    }
}