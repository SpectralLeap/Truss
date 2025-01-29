namespace Truss.Monads.Results.Extensions.Fluent;

/// <summary>
/// Extension methods for <see cref="ResolutionStep{T}"/>
/// </summary>
public static class ResolutionStepExtensions
{
    /// <summary>
    /// Converts a <see cref="ResolutionStep{T}"/> to a <see cref="Result{T}"/>
    /// </summary>
    /// <param name="resolutionStep">
    /// The resolution step to convert
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> AsResult<T>(this Task<ResolutionStep<T>> resolutionStep)
    {
        return await resolutionStep;
    }
}