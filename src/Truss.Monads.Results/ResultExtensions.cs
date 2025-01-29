namespace Truss.Monads.Results;

/// <summary>
/// Extension methods for handling Result objects
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a value to a Result
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> AsResult<T>(this T obj)
    {
        return Result.Success(obj);
    }
}