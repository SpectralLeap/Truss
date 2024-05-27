using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Truss.Modeling.Installation.Configuration;

public sealed class EmptyConfigurationSection : IConfigurationSection
{
    private readonly IConfiguration _root;
    private readonly string _path;

    public EmptyConfigurationSection(IConfiguration root, string path)
    {
        _root = root;
        _path = path;
    }

    public string this[string key]
    {
        get => throw new EmptyConfigurationInteractionException();
        set => throw new EmptyConfigurationInteractionException();
    }

    public string Key => _path.Split(':').Last();

    public string Path => _path;

    public string Value
    {
        get => throw new EmptyConfigurationInteractionException();
        set => throw new EmptyConfigurationInteractionException();
    }

    public IEnumerable<IConfigurationSection> GetChildren() => throw new EmptyConfigurationInteractionException();

    public IChangeToken GetReloadToken() => NullChangeToken.Instance;

    public IConfigurationSection GetSection(string key) => throw new EmptyConfigurationInteractionException();
}