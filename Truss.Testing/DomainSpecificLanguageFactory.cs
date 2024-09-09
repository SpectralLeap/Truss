using System.Reflection;
using Castle.DynamicProxy;
using Truss.Testing.Services;

namespace Truss.Testing;

/// <summary>
/// Represents a factory for creating DomainSpecificLanguage instances.
/// The lifetime of this type should be singleton.
/// </summary>
public sealed class DomainSpecificLanguageFactory 
    : IAsyncDisposable
{
    private readonly ProxyGenerator _proxyGenerator = new();
    
    private DomainSpecificLanguageManager? _domainSpecificLanguageManager;
    private SharedDependencyManager? _sharedDependencyManager;
    
    private bool _disposing;

    /// <summary>
    /// Creates a new domain specific language factory
    /// </summary>
    public DomainSpecificLanguageFactory()
    {
        _domainSpecificLanguageManager = new DomainSpecificLanguageManager(
            _proxyGenerator
        );
    }
    
    /// <summary>
    /// Asynchronously initializes the DomainSpecificLanguage factory by starting external dependencies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for external dependencies.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(
        IEnumerable<Assembly> assemblies
    )
    {
        _sharedDependencyManager = new SharedDependencyManager(assemblies);
        await _sharedDependencyManager.Start();

        _domainSpecificLanguageManager = new DomainSpecificLanguageManager(
            _sharedDependencyManager,
            _proxyGenerator
        );
    }


    /// <summary>
    /// Retrieves an instance of a DomainSpecificLanguage based on the specified ID and tags.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDomainSpecificLanguage">The type of the DomainSpecificLanguage to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the DomainSpecificLanguage instance to retrieve. If not provided,
    /// a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply DomainSpecificLanguage service overrides.</param>
    /// <returns>An instance of the specified DomainSpecificLanguage type.</returns>
    public TDomainSpecificLanguage GetDomainSpecificLanguage<TDomainSpecificLanguage>(string? id = null, params string[] tags)
        where TDomainSpecificLanguage : DomainSpecificLanguage
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return _domainSpecificLanguageManager!.ClassProxyWithTargetAsync<TDomainSpecificLanguage>(id, tags)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Retrieves an instance of a DomainSpecificLanguage based on the specified ID and tags and initializes it asynchronously.
    /// If an ID is not provided, a new GUID is generated.
    /// </summary>
    /// <typeparam name="TDomainSpecificLanguage">The type of the DomainSpecificLanguage to retrieve.</typeparam>
    /// <param name="id">Optional. The ID of the DomainSpecificLanguage instance to retrieve. If not provided, a new GUID will be generated.</param>
    /// <param name="tags">Optional. An array of tags to apply DomainSpecificLanguage service overrides.</param>
    /// <returns>An instance of the specified DomainSpecificLanguage type.</returns>
    public async Task<TDomainSpecificLanguage> GetDomainSpecificLanguageAsync<TDomainSpecificLanguage>(string? id = null, params string[] tags)
        where TDomainSpecificLanguage : DomainSpecificLanguage
    {
        id ??= string.Join("", Guid.NewGuid().ToString().Take(5));

        return await _domainSpecificLanguageManager!.ClassProxyWithTargetAsync<TDomainSpecificLanguage>(id, tags);
    }


    /// <summary>
    /// Dispose of all resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposing) return;

        _disposing = true;

        await _sharedDependencyManager!.DisposeAsync();
        await _domainSpecificLanguageManager!.DisposeAsync();
    }
}