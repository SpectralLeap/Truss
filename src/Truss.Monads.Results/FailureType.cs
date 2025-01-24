namespace Truss.Monads.Results;

/// <summary>
/// The type of failure that occurred
/// </summary>
public enum FailureType
{
    /// <summary>
    /// The operation failed to complete due to a non-exceptional condition
    /// </summary>
    /// <example>
    /// A precondition was not met
    /// </example>
    /// <example>
    /// A required resource was not found
    /// </example>
    /// <example>
    /// The request was poorly formed
    /// </example>
    Failed,

    /// <summary>
    /// An unexpected error occurred during execution
    /// </summary>
    /// <example>
    /// An exception was thrown
    /// </example>
    Error,

    /// <summary>
    /// The operation could not proceed because the user was unauthenticated
    /// </summary>
    /// <example>
    /// Authentication credentials were missing or invalid
    /// </example>
    Unauthenticated,

    /// <summary>
    /// The operation could not proceed because the user was unauthorized
    /// </summary>
    /// <example>
    /// The user lacks the required permissions or roles
    /// </example>
    Unauthorized,

    /// <summary>
    /// The operation failed due to validation errors in input or state
    /// </summary>
    /// <example>
    /// A guid field was not a valid guid
    /// </example>
    /// <example>
    /// A string field was too long
    /// </example>
    /// <example>
    /// A required field was missing
    /// </example>
    Validation,

    /// <summary>
    /// The operation was cancelled before it could complete
    /// </summary>
    /// <example>
    /// A user cancelled a long-running operation
    /// </example>
    /// <example>
    /// A timeout occurred
    /// </example>
    Cancelled,
}