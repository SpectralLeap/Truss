using System.Reflection;

namespace Truss.Configuration;

public sealed class TrussConfig
{
    private readonly List<Assembly> _assembliesToLoadFrom = new();

    internal IReadOnlyCollection<Assembly> Assemblies => _assembliesToLoadFrom.AsReadOnly();
    
    public TrussConfig UsingAssembly(Assembly assembly)
    {
        _assembliesToLoadFrom.Add(assembly);
        return this;
    }
}