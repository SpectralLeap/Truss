namespace Truss.Monads.Results;

/// <summary>
/// A result was in an invalid state
/// </summary>
public sealed class InvalidResultStateException() 
    : InvalidOperationException("Success evaluated to null. Result is in an invalid state");