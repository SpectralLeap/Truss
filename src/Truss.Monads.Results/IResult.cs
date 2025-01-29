namespace Truss.Monads.Results;

/// <inheritdoc />
/// The type of the successful result object
/// <typeparam name="TSuccess"></typeparam>
public interface IResult<out TSuccess> : IResult
{
    /// <summary>
    /// Get the success value if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public TSuccess SuccessValue { get; }
}

/// <summary>
/// A basic result type that reports success or failure reasons
/// </summary>
public interface IResult
{
    /// <summary>
    /// True if the operation succeeded
    /// </summary>
    public bool Succeeded { get; }
    
    /// <summary>
    /// True if the operation failed
    /// </summary>
    public bool Failed { get; }
    
    /// <summary>
    /// Get the success value as an object if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public object? SuccessObject { get; }
    
    /// <summary>
    /// Get the failure details if failed else throws
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving failure value on a successful operation</exception>
    public FailureDetails? FailureDetails { get; }
    
    /// <summary>
    /// Get the failure message if failed else throws
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving failure value on a successful operation</exception>
    public string FailureMessage { get; }
}