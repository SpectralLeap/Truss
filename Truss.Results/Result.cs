#pragma warning disable CS0108, CS0114

namespace Truss.Results;

/// <summary>
/// A basic result type that reports success or invalidation reasons
/// </summary>
public static class Result
{
    /// <summary>
    /// Create success
    /// </summary>
    /// <returns></returns>
    public static Result<None> Success() => new(None.Value);

    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Result<T> Success<T>(T result) => new(result);

    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons"></param>
    /// <returns></returns>
    public static Result<None> Fail(params string[] reasons) => new(FailureDetails.From(reasons));

    /// <summary>
    /// Create failure from exception
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<None> Fail(Exception ex) => new(FailureDetails.From(ex));

    /// <summary>
    /// Create failure from exception with a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<None> Fail(string message, Exception ex) => new(FailureDetails.From(ex, message));

    /// <summary>
    /// Create failure from details
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static Result<None> Fail(FailureDetails details) => new(details);
}

/// <summary>
/// A basic result that returns a value
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed record Result<TResult> : IResult
{
    
    private readonly TResult? _success;
    private readonly FailureDetails? _failure;

    /// <summary>
    /// True if the operation succeeded
    /// </summary>
    public bool Succeeded => _success is not null;
    
    /// <summary>
    /// True if the operation failed
    /// </summary>
    public bool Failed => _failure is not null;

    /// <summary>
    /// Get the success value if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public TResult SuccessValue =>
        _success ?? throw new InvalidOperationException($"Tried to get success value on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
    
    /// <summary>
    /// Get the failure details if failed
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving failure value on a successful operation</exception>
    public FailureDetails FailureDetails => 
        _failure ?? throw new InvalidOperationException($"Tried to get failure value on successful operation");
    
    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="success"></param>
    internal Result(TResult success)
    {
        // ReSharper disable once NotResolvedInText
        _success = success ?? throw new ArgumentNullException("Success value was null"); 
    }
    
    /// <summary>
    /// Create failure
    /// </summary>
    /// <param name="failure"></param>
    internal Result(FailureDetails failure)
    {
        // ReSharper disable once NotResolvedInText
        _failure = failure ?? throw new ArgumentNullException("Failure details were null");
    }

    public static implicit operator Result<TResult>(TResult value) => new(value);
    
    public static implicit operator Result<TResult>(Result<None> unit) => new(unit.FailureDetails);
    
    /// <summary>
    /// Without an operation it is safe to implicitly cast
    /// this for readability
    /// </summary>
    /// <returns></returns>
    public static implicit operator Task<Result<TResult>>(Result<TResult> result) => Task.FromResult(result);
}

public interface IResult
{
    public bool Succeeded { get; }
    public bool Failed { get; }
    public FailureDetails? FailureDetails { get; }
}