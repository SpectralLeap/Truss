using System.Diagnostics.CodeAnalysis;

namespace Truss.Modeling.Domain.EventSourcing;

/// <summary>
/// Represents a sequence number for events.
/// </summary>
public sealed record EventSequenceNumber : IComparable<EventSequenceNumber>
{
    /// <summary>
    /// Represents a sequence number for events.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Represents a sequence number for events.
    /// </summary>
    public EventSequenceNumber(long value)
    {
        Value = value;
    }
     
    /// <summary>
    /// Overloads the '+' operator to perform addition between an EventSequenceNumber and a long value.
    /// </summary>
    /// <param name="a">The EventSequenceNumber to add to.</param>
    /// <param name="b">The long value to add.</param>
    /// <returns>A new EventSequenceNumber that results from adding the long value to the original number.</returns>
    public static EventSequenceNumber operator +(EventSequenceNumber a, long b) => new(a.Value + b);

    /// <summary>
    /// Determines whether the specified EventSequenceNumber is equal to the specified integer.
    /// </summary>
    /// <param name="a">The EventSequenceNumber to compare.</param>
    /// <param name="b">The integer to compare against.</param>
    /// <returns>
    /// <c>true</c> if the EventSequenceNumber is equal to the integer; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(EventSequenceNumber a, int b) => a.Value == b;

    /// <summary>
    /// Overloaded inequality operator for comparing an EventSequenceNumber object with an integer.
    /// </summary>
    /// <param name="a">The EventSequenceNumber object to compare.</param>
    /// <param name="b">The integer to compare.</param>
    /// <returns>True if the EventSequenceNumber object is not equal to the integer, false otherwise.</returns>
    public static bool operator !=(EventSequenceNumber a, int b) => !(a == b);

    /// <summary>
    /// Compares the current instance of <see cref="EventSequenceNumber"/> to
    /// another <see cref="EventSequenceNumber"/> object.
    /// </summary>
    /// <param name="other">The <see cref="EventSequenceNumber"/> object to compare with
    /// the current instance.</param>
    /// <returns>
    /// A signed integer that indicates the relative order of the objects being compared.
    /// The return value is <c>0</c> if the current instance is equal to <paramref name="other"/>;
    /// a value less than <c>0</c> if the current instance is less than <paramref name="other"/>;
    /// and a value greater than <c>0</c> if the current instance is greater than <paramref name="other"/>.
    /// </returns>
    /// <remarks>
    /// This method compares the <see cref="Value"/> property of each object.
    /// If <paramref name="other"/> is <c>null</c>, the current instance is considered greater.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public int CompareTo(EventSequenceNumber? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}