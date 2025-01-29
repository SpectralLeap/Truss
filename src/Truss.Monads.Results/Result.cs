#pragma warning disable CS0108, CS0114

namespace Truss.Monads.Results;

/// <summary>
/// A basic result type that reports success or failure reasons
/// </summary>
public static class Result
{
    /// <summary>
    /// Create success
    /// </summary>
    /// <returns></returns>
    public static Result<Nil> Success() => Result<Nil>.Success(Nil.Value);

    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Result<T> Success<T>(T result) => Result<T>.Success(result);

    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons"></param>
    /// <returns></returns>
    public static Result<Nil> Fail(params string[] reasons) => Result<Nil>.Fail(FailureDetails.From(reasons));
    
    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons"></param>
    /// <returns></returns>
    public static Result<Nil> Fail(IEnumerable<string> reasons) => Result<Nil>.Fail(FailureDetails.From(reasons.ToArray()));

    /// <summary>
    /// Create failure from exception
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<Nil> Fail(Exception ex) => Result<Nil>.Fail(FailureDetails.From(ex));

    /// <summary>
    /// Create failure from exception with a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<Nil> Fail(string message, Exception ex) => Result<Nil>.Fail(FailureDetails.From(ex, message));

    /// <summary>
    /// Create failure from details
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static Result<Nil> Fail(FailureDetails details) => Result<Nil>.Fail(details);
}

/// <inheritdoc />
public readonly struct Result<TSuccess> : IResult
{
    private readonly TSuccess? _successValue;
    private readonly FailureDetails? _failureDetails;

    /// <inheritdoc />
    public bool Succeeded { get; }

    /// <inheritdoc />
    public bool Failed => !Succeeded;

    /// <summary>
    /// Get the success value if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public TSuccess SuccessValue =>
        _successValue ?? throw new InvalidOperationException($"Tried to get success value on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
    
    /// <inheritdoc />
    public object SuccessObject =>
        _successValue ?? throw new InvalidOperationException($"Tried to get success object on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
     
    /// <inheritdoc />
    public FailureDetails FailureDetails => 
        _failureDetails ?? throw new InvalidOperationException($"Tried to get failure value on successful operation");

    /// <inheritdoc />
    public string FailureMessage => FailureDetails.GetMessage();

    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="success"></param>
    /// <exception cref="ArgumentNullException">If the success object is null</exception>
    private Result(TSuccess success)
    {
        _successValue = success ?? throw new ArgumentNullException(
            nameof(success),
            $"A Result value of type {typeof(TSuccess).Name} cannot be null"
        );
        Succeeded = true;
    }
    
    /// <summary>
    /// Create failure
    /// </summary>
    /// <param name="failure"></param>
    private Result(FailureDetails failure)
    {
        // ReSharper disable once NotResolvedInText
        Succeeded = false;
        _failureDetails = failure ?? throw new ArgumentNullException(nameof(failure));
    }

    /// <summary>
    /// Resolve the result and return a value
    /// </summary>
    /// <param name="onSuccess">
    /// The function to call if the operation succeeded
    /// </param>
    /// <param name="onFailure">
    /// The function to call if the operation failed
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the output
    /// </typeparam>
    /// <returns></returns>
    public TOut Resolve<TOut>(
        Func<TSuccess, TOut> onSuccess,
        Func<FailureDetails, TOut> onFailure
    )
    {
        if (Succeeded) return onSuccess(SuccessValue);
        return onFailure(FailureDetails);
    }
    
    /// <summary>
    /// Resolve the result
    /// </summary>
    /// <param name="onSuccess">
    /// The function to call if the operation succeeded
    /// </param>
    /// <param name="onFailure">
    /// The function to call if the operation failed
    /// </param>
    public void Resolve(
        Action<TSuccess> onSuccess,
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
    /// Create success
    /// </summary>
    /// <returns></returns>
    public static Result<Nil> Success() => new(Nil.Value);

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
    public static Result<TSuccess> Fail(params string[] reasons) => new(FailureDetails.From(reasons));

    /// <summary>
    /// Create failure from exception
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<TSuccess> Fail(Exception ex) => new(FailureDetails.From(ex));

    /// <summary>
    /// Create failure from exception with a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static Result<TSuccess> Fail(string message, Exception ex) => new(FailureDetails.From(ex, message));

    /// <summary>
    /// Create failure from details
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static Result<TSuccess> Fail(FailureDetails details) => new(details);
         
    /// <summary>
    /// Returns the success value or a default value
    /// </summary>
    /// <param name="result"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TSuccess? operator | (in Result<TSuccess> result, TSuccess? defaultValue)
    {
        if (result.Succeeded) return result.SuccessValue;
        
        return defaultValue;
    }
    
    /// <summary>
    /// Implicit cast for readability
    /// </summary>
    /// <returns></returns>   
    public static implicit operator Result<TSuccess>(Result<Nil> value)
    {
        if (value.Succeeded) throw new InvalidCastException($"Cannot cast {nameof(Nil)} to {nameof(TSuccess)}");

        return new Result<TSuccess>(value.FailureDetails);
    }
}