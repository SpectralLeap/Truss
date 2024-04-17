using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Modeling.Infrastructure;

public interface ITrussServiceInstaller
{
    public void InstallServices(TrussDependencyModel trussDependencyModel);
}

public sealed class TypeResolutionBuilder
{
    public TypeResolutionBuilder2 FromInterface(Type typeInterface)
    {
        return new TypeResolutionBuilder2(typeInterface);
    }
}

public class TypeResolutionBuilder2
{
    private readonly Type _typeInterface;
    private ServiceLifetime _serviceLifetime = ServiceLifetime.Transient;

    public TypeResolutionBuilder2(Type typeInterface)
    {
        _typeInterface = typeInterface;
    }

    public TypeResolutionBuilder2 WithServiceLifetime(ServiceLifetime serviceLifetime)
    {
        _serviceLifetime = serviceLifetime;
        
        return this;
    }

    public ServiceDescriptor Build()
    {
        
    }
}

public sealed class TrussDependencyModel
{
    private readonly IServiceCollection _services;
    private readonly Assembly[] _assemblies;
    
    public TrussDependencyModel(
        IServiceCollection services,
        Assembly[] assemblies
    )
    {
        _services = services;
        _assemblies = assemblies;
    }

    public TrussDependencyModel Add(Action<IServiceCollection> add)
    {
        add(_services);
        return this;
    }

    public TrussDependencyModel Add(Action<IServiceCollection, Assembly[]> add)
    {
        add(_services, _assemblies);
        return this;
    }
    
    public TrussDependencyModel AddTransient<TInterface, TService>() 
        where TInterface : class 
        where TService : class, TInterface
    {
        _services.AddTransient<TInterface, TService>();

        return this;
    }
    
    public TrussDependencyModel CloseAllTypesOf(
        Type typeInterface,
        ServiceLifetime lifetime = ServiceLifetime.Transient
    )
    {
        var types = Types(typeInterface);

        foreach (var closedType in types)
        {
            Close(typeInterface, lifetime, closedType);
        }

        return this;
    }

    private void Close(Type typeInterface, ServiceLifetime lifetime, Type closedType)
    {
        var typeArguments = closedType
            .GetInterfaces()
            .First(t => TypeImplements(t, typeInterface))
            .GetGenericArguments();

        var genericType = typeInterface.MakeGenericType(typeArguments);

        _services.Add(new ServiceDescriptor(
            genericType,
            closedType,
            lifetime
        ));
    }

    public TrussDependencyModel WrapAllTypesOf(
        Type innerType,
        Type wrapperType,
        Type wrapperHandlerType,
        Type wrapperHandlerInterfaceType,
        Type returnType
    )
    {
        var types = Types(innerType);

        foreach (var type in types)
        {
            Wrap(
                type,
                innerType,
                wrapperType,
                wrapperHandlerType,
                wrapperHandlerInterfaceType,
                returnType
            );
        }

        return this;
    }

    private void Wrap(
        Type innerType,
        Type typeInterface,
        Type wrapperType,
        Type wrapperHandlerType,
        Type wrapperHandlerInterfaceType,
        Type resultType = null
    )
    {
        var typeArguments = innerType
            .GetInterfaces()
            .First(t => TypeImplements(t, typeInterface))
            .GetGenericArguments();

        var wrapper = wrapperType.MakeGenericType(typeArguments);
        var wrappedHandlerType = wrapperHandlerType.MakeGenericType(typeArguments);

        var returnType = resultType.IsGenericType ? resultType.MakeGenericType(typeArguments.Last()) : resultType;
        
        var wrappedGenType = wrapperHandlerInterfaceType.MakeGenericType(wrapper, returnType);
        
        _services.Add(new ServiceDescriptor(
            wrappedGenType, wrappedHandlerType, ServiceLifetime.Transient
        ));
    }

    private IEnumerable<Type> Types(Type typeInterface)
    {
        var closedTypes = _assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass 
                            && !t.IsAbstract 
                            && t.GetInterfaces().Any(i =>
                                i.IsGenericType 
                                && TypeImplements(i, typeInterface)
                            )
                )
            ;
        return closedTypes;
    }

    private static bool TypeImplements(Type type, Type typeInterface)
    {
        return type.GetGenericTypeDefinition() == typeInterface;
    }
}