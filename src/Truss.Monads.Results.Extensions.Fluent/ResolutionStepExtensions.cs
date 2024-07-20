namespace Truss.Monads.Results.Extensions.Fluent;

public static class ResolutionStepExtensions
{
    public static async Task<Result<T>> AsResult<T>(this Task<ResolutionStep<T>> resolutionStep)
    {
        return await resolutionStep;
    }
}