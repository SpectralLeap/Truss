using System.Reflection;
using Castle.DynamicProxy;
using Truss.Testing.Services;

namespace Truss.Testing;

/// <summary>
/// Represents a factory for creating Fixture instances.
/// The lifetime of this type should be singleton.
/// </summary>
public sealed class FixtureFactory 
    : IAsyncDisposable
{
    private readonly ProxyGenerator _proxyGenerator = new();
    
    private FixtureManager? _fixtureManager;
    private SharedDependencyManager? _sharedDependencyManager;
    
    private bool _disposing;

    public FixtureFactory()
    {
        _fixtureManager = new FixtureManager(
            _proxyGenerator
        );
    }
    
    /// <summary>
    /// Asynchronously initializes the Fixture factory by starting external dependencies.
    /// </summary>
    /// <param name="assembly">The assembly to scan for external dependencies</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(
        Assembly assembly
    )
    {
        await InitializeAsync([ assembly ]);
    }
    
    /// <summary>
    /// Asynchronously initializes the Fixture factory by starting external dependencies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for external dependencies.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(
        IEnumerable<Assembly> assemblies
    )
    {
        _sharedDependencyManager = new SharedDependencyManager(assemblies);
        await _sharedDependencyManager.Start();

        _fixtureManager = new FixtureManager(
            _sharedDependencyManager,
            _proxyGenerator
        );
    }


    /// <summary>
    /// Retrieves an instance of a Fixture based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TFixture">The type of the Fixture to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the Fixture instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply Fixture service overrides.</param>
    /// <returns>An instance of the specified Fixture type.</returns>
    public TFixture GetFixture<TFixture>(string? id = null, params string[] tags) where TFixture : Fixture
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return _fixtureManager!.ClassProxyWithTarget<TFixture>(id, tags);
    }



    /// <summary>
    /// Dispose of all resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposing) return;

        _disposing = true;

        await _sharedDependencyManager!.DisposeAsync();
        await _fixtureManager!.DisposeAsync();
    }
}

