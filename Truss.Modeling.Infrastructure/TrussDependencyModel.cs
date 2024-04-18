using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure;

public sealed class TrussDependencyModel
{
    private readonly IServiceCollection _services;
    private readonly Type[] _types;
    
    public TrussDependencyModel(
        IServiceCollection services,
        Type[] types
    )
    {
        _services = services;
        _types = types;
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
}