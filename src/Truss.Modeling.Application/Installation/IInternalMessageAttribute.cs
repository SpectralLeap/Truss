namespace Truss.Modeling.Application.Installation;

/// <summary>
/// Marks a message that should not be exposed on the API
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class InternalMessageAttribute : Attribute
{
    
}
