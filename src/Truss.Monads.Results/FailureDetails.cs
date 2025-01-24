namespace Truss.Monads.Results;

/// <summary>
/// A record that contains details about a failure
/// </summary>
public sealed record FailureDetails
{
    private readonly string[] _failureReasons;
    /// <summary>
    /// The reasons for failure
    /// </summary>
    public IReadOnlyCollection<string> FailureReasons => _failureReasons;

    /// <summary>
    /// A message that joins the failure reasons
    /// </summary>
    /// <param name="joinDelimiter">
    /// The delimiter to join the reasons with
    /// </param>
    /// <returns></returns>
    public string GetMessage(string joinDelimiter = ". ") => string.Join(joinDelimiter, _failureReasons);

    /// <summary>
    /// The exception that caused the failure if there was one
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// The type of failure that occurred
    /// </summary>
    public FailureType FailureType { get; private set; }

    /// <summary>
    /// The code that represents the failure
    /// </summary>
    public int? FailureCode { get; private set; }

    private FailureDetails(
        FailureType failureType,
        string[] failureReasons,
        int? failureCode = null
    )
    {
        FailureType = failureType;
        _failureReasons = failureReasons;
        FailureCode = failureCode;
    }

    private FailureDetails(
        FailureType failureType,
        Exception exception,
        string[] failureReasons,
        int? failureCode = null
    ) : this(
        failureType,
        failureReasons,
        failureCode
    )
    {
        Exception = exception;
    }

    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        params string[] reasons
    )
    {
        return new FailureDetails(
            FailureType.Failed,
            reasons
        );
    }

    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <param name="failureType">
    /// The type of failure that occurred (Default: <see cref="FailureType.Failed"/>)
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        FailureType failureType,
        params string[] reasons
    )
    {
        return new FailureDetails(
            failureType,
            reasons
        );
    }


    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <param name="failureType">
    /// The type of failure that occurred (Default: <see cref="FailureType.Failed"/>)
    /// </param>
    /// <param name="failureCode">
    /// The code that represents the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        FailureType failureType,
        int failureCode,
        params string[] reasons
    )
    {
        return new FailureDetails(
            failureType,
            reasons,
            failureCode
        );
    }


    /// <summary>
    /// Create failure from an exception
    /// </summary>
    /// <param name="exception">
    /// The exception that caused the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        Exception exception
    )
    {
        var reasons = new string[] {};
        
        return From(exception, reasons);
    }

    /// <summary>
    /// Create failure from an exception and reasons
    /// </summary>
    /// <param name="exception">
    /// The exception that caused the failure
    /// </param>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        Exception exception,
        params string[] reasons
    )
    {
        var reasonsList = reasons.ToList();
        reasonsList.AddRange([
            exception.Message,
            exception.InnerException?.Message ?? "No inner exception"
        ]);
        
        return new FailureDetails(
            FailureType.Error,
            exception,
            reasonsList.ToArray()
        );
    }

    /// <summary>
    /// Create failure from an exception and a message
    /// </summary>
    /// <param name="exception">
    /// The exception that caused the failure
    /// </param>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        OperationCanceledException exception,
        params string[] reasons
    )
    {
        var reasonsList = reasons.ToList();
        reasonsList.AddRange([
            exception.Message,
            exception.InnerException?.Message ?? "No inner exception"
        ]);

        return new FailureDetails(
            FailureType.Cancelled,
            exception,
            reasonsList.ToArray()
        );
    }

    /// <summary>
    /// Create failure from an exception and a message
    /// </summary>
    /// <param name="exception">
    /// The exception that caused the failure
    /// </param>
    /// <param name="reasons">
    /// The reasons for the failure
    /// </param>
    /// <returns></returns>
    public static FailureDetails From(
        TimeoutException exception,
        params string[] reasons
    )
    {
        var reasonsList = reasons.ToList();
        reasonsList.AddRange([
            exception.Message,
            exception.InnerException?.Message ?? "No inner exception"
        ]);

        return new FailureDetails(
            FailureType.Cancelled,
            exception,
            reasonsList.ToArray()
        );
    }
}