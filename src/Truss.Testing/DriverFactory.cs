using System.Reflection;
using Truss.Testing.Services;

namespace Truss.Testing;

/// <summary>
/// Represents a factory for creating Driver instances.
/// The lifetime of this type should be singleton.
/// </summary>
public sealed class DriverFactory
    : IAsyncDisposable
{
    private DriverManager _driverManager;
    private SharedDependencyManager? _sharedDependencyManager;

    private bool _disposing;

    /// <summary>
    /// Creates a new driver factory
    /// </summary>
    public DriverFactory()
    {
        _driverManager = new DriverManager();
    }

    /// <summary>
    /// Asynchronously initializes the Driver factory by starting external dependencies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for external dependencies.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(
        Assembly[] assemblies
    )
    {
        _sharedDependencyManager = new SharedDependencyManager(assemblies);
        await _sharedDependencyManager.Start();

        _driverManager = new DriverManager(
            _sharedDependencyManager
        );
    }


    /// <summary>
    /// Retrieves an instance of a Driver based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDriver">The type of the Driver to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the Driver instance to retrieve. If not provided,
    /// a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply Driver service overrides.</param>
    /// <returns>An instance of the specified Driver type.</returns>
    public TDriver GetDriver<TDriver>(string? id = null, params string[] tags)
        where TDriver : Driver
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return _driverManager.ClassProxyWithTargetAsync<TDriver>(id, tags)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Retrieves an instance of a Driver based on the specified ID and tags and initializes it asynchronously.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDriver">The type of the Driver to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the Driver instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply Driver service overrides.</param>
    /// <returns>An instance of the specified Driver type.</returns>
    public async Task<TDriver> GetDriverAsync<TDriver>(string? id = null, params string[] tags)
        where TDriver : Driver
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return await _driverManager.ClassProxyWithTargetAsync<TDriver>(id, tags);
    }


    /// <summary>
    /// Dispose of all resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposing) return;

        _disposing = true;

        await _sharedDependencyManager!.DisposeAsync();
        await _driverManager!.DisposeAsync();
    }
}