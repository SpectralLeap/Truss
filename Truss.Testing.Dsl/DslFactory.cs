using System.Reflection;
using Castle.DynamicProxy;
using Truss.Testing.Dsl.Services;

namespace Truss.Testing.Dsl;

/// <summary>
/// Represents a factory for creating DSL (Domain-Specific Language) instances.
/// The lifetime of this type should be singleton.
/// </summary>
public sealed class DslFactory 
    : IAsyncDisposable
{
    private readonly ProxyGenerator _proxyGenerator = new();
    
    private DslManager? _dslManager;
    private SharedDependencyManager? _sharedDependencyManager;
    
    private bool _disposing;

    /// <summary>
    /// Asynchronously initializes the DSL factory by starting external dependencies.
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
    /// Asynchronously initializes the DSL factory by starting external dependencies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for external dependencies.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(
        IEnumerable<Assembly> assemblies
    )
    {
        _sharedDependencyManager = new SharedDependencyManager(assemblies);
        await _sharedDependencyManager.Start();

        _dslManager = new DslManager(
            _sharedDependencyManager,
            _proxyGenerator
        );
    }


    /// <summary>
    /// Retrieves an instance of a DSL (Domain Specific Language) based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDsl">The type of the DSL to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the DSL instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply DSL service overrides.</param>
    /// <returns>An instance of the specified DSL type.</returns>
    public TDsl GetDsl<TDsl>(string? id = null, params string[] tags) where TDsl : Dsl
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return _dslManager.ClassProxyWithTarget<TDsl>(id, tags);
    }



    /// <summary>
    /// Dispose of all resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposing) return;

        _disposing = true;

        await _sharedDependencyManager!.DisposeAsync();
        await _dslManager.DisposeAsync();
    }
}

