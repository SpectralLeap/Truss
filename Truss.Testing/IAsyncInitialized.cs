namespace Truss.Testing;

/// <summary>
/// Marks a Domain Specific Language that initializes asynchronously
/// </summary>
public interface IAsyncInitialized
{
    /// <summary>
    /// The asynchronous initialization
    /// </summary>
    /// <returns></returns>
    public ValueTask InitializeAsync();
}