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
public readonly struct Result<TResult> : IResult
{
    private readonly TResult? _successValue;
    private readonly FailureDetails? _failureDetails;

    /// <summary>
    /// True if the operation succeeded
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// True if the operation failed
    /// </summary>
    public bool Failed => !Succeeded;

    /// <summary>
    /// Get the success value if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public TResult SuccessValue =>
        _successValue ?? throw new InvalidOperationException($"Tried to get success value on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
    
    /// <summary>
    /// Get the success value as an object if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public object SuccessObject =>
        _successValue ?? throw new InvalidOperationException($"Tried to get success object on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
     
    /// <summary>
    /// Get the failure details if failed else throws
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving failure value on a successful operation</exception>
    public FailureDetails FailureDetails => 
        _failureDetails ?? throw new InvalidOperationException($"Tried to get failure value on successful operation");

    /// <summary>
    /// Get the failure message if failed else throws
    /// </summary>
    public string FailureMessage => FailureDetails.GetMessage();

    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="success"></param>
    /// <exception cref="ArgumentNullException">If the success object is null</exception>
    internal Result(TResult success)
    {
        _successValue = success ?? throw new ArgumentNullException(nameof(success)); 
        Succeeded = true;
    }
    
    /// <summary>
    /// Create failure
    /// </summary>
    /// <param name="failure"></param>
    internal Result(FailureDetails failure)
    {
        // ReSharper disable once NotResolvedInText
        Succeeded = false;
        _failureDetails = failure ?? throw new ArgumentNullException(nameof(failure));
    }

    public TOut Resolve<TOut>(
        Func<TResult, TOut> onSuccess,
        Func<FailureDetails, TOut> onFailure
    )
    {
        if (Succeeded) return onSuccess(SuccessValue);
        return onFailure(FailureDetails);
    }
    
    public void Resolve(
        Action<TResult> onSuccess,
        Action<FailureDetails> onFailure
    )
    {
        if (Succeeded)
        {
            onSuccess(SuccessValue);
            return;
        }
        onFailure(FailureDetails);
    }
     
    /// <summary>
    /// Returns the success value or a default value
    /// </summary>
    /// <param name="result"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TResult? operator | (in Result<TResult> result, TResult? defaultValue)
    {
        if (result.Succeeded) return result.SuccessValue;
        
        return defaultValue;
    }


    /// <summary>
    /// Implicit cast for readability
    /// </summary>
    /// <returns></returns>   
    public static implicit operator Result<TResult>(Result<None> value)
    {
        if (value.Succeeded) throw new InvalidCastException($"Cannot cast {nameof(None)} to {nameof(TResult)}");

        return new Result<TResult>(value.FailureDetails);
    }
 
    /// <summary>
    /// Implicit cast for readability
    /// </summary>
    /// <returns></returns>
    public static implicit operator Task<Result<TResult>>(Result<TResult> result) => Task.FromResult(result);
}

public interface IResult
{
    public bool Succeeded { get; }
    public object SuccessObject { get; }
    public bool Failed { get; }
    public FailureDetails? FailureDetails { get; }
}