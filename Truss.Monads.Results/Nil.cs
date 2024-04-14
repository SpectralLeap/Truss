namespace Truss.Monads.Results;

/// <summary>
/// Represents an explicit empty value
/// for when no type is returned
/// </summary>
public struct Nil
{
    public static Nil Value { get; } = new();
};