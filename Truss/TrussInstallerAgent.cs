using System.Reflection;

namespace Truss;

public sealed class TrussInstallerAgent
{
    private readonly Type[] _types;
    
    public TrussInstallerAgent(
        Assembly[] assemblies
    )
    {
        _types = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .ToArray()
            ;
    }


    public void ForAllOfType<T>(Action<Type> func)
    {
        var types = GetTypes(typeof(T));
    
        foreach (var type in types)
        {
            func(type);
        }
    }
    
    public void InvokeAll<T>(Action<T> func)
    {
        var types = GetTypes(typeof(T));
        
        foreach (var type in types)
        {
            var instance = (T)Activator.CreateInstance(type);
            func(instance);
        }
    }
    
    private IEnumerable<Type> GetTypes(Type typeInterface)
    {
        var closedTypes = _types
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                TypeImplements(i, typeInterface)
                            )
                )
            ;
        return closedTypes;
    }
    
    private static bool TypeImplements(Type type, Type typeInterface)
    {
        return type == typeInterface;
    }

}