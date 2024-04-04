namespace Truss.Monads.Results;

public static class ResultExtensions
{
    public static Result<T> AsResult<T>(this T obj)
    {
        return Result.Success(obj);
    }
}