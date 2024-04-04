namespace Truss.Testing.Dsl.SharedDependencies;

/// <summary>
/// Represents a global service.
/// </summary>
public interface ISharedDependency : IAsyncDisposable
{
    /// <summary>
    /// Starts the service
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StartAsync();
}