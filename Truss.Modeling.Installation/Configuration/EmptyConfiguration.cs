using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Truss.Modeling.Installation.Configuration;

public sealed class EmptyConfiguration : IConfiguration
{
    public string this[string key]
    {
        get => throw new EmptyConfigurationInteractionException();
        set => throw new EmptyConfigurationInteractionException();
    }

    public IConfigurationSection GetSection(string key) => new EmptyConfigurationSection(this, key);
    
    public IEnumerable<IConfigurationSection> GetChildren() => throw new EmptyConfigurationInteractionException();

    public IChangeToken GetReloadToken() => NullChangeToken.Instance;

}