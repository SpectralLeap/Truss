namespace Truss.Monads.Results;

/// <summary>
/// Represents an explicit empty value
/// for when no type is returned
/// </summary>
public struct Nil
{
    /// <summary>
    /// The value of the Nil type
    /// </summary>
    public static Nil Value { get; } = new();
};