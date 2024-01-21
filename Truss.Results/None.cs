namespace Truss.Results;

/// <summary>
/// Represents an explicit empty value
/// for when no type is returned
/// </summary>
public struct None
{
    public static None Value { get; } = new();
};