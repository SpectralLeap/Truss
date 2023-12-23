using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Truss.Dsl;

internal sealed class ServiceDefinitions
{
    private readonly Dictionary<string, ServiceDefinition> _serviceOverrides = [];
    private readonly List<ServiceDefinition> _serviceDefinitions = [];

    private void Add(ServiceDefinition serviceDefinition)
    {
        if (serviceDefinition.Tag is not null)
        {
            _serviceOverrides.Add(serviceDefinition.Tag, serviceDefinition);
            return;
        }
        
        _serviceDefinitions.Add(serviceDefinition);
    }

    public static ServiceDefinitions For<TDsl>()
    {
        var serviceDefinitions = new List<ServiceDefinition>();
        
        serviceDefinitions.AddRange( typeof(TDsl)
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(IsServiceDefinition)
            .Where(AssertStatic)
            .Select(ParseDefinition)
            .ToList()
        );
            
        serviceDefinitions.AddRange(typeof(TDsl)
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(IsServiceDefinition)
            .Where(AssertStatic)
            .Select(ParseDefinition)
            .ToList()
        );

        var s = new ServiceDefinitions();
        foreach (var serviceDefinition in serviceDefinitions)
        {
            s.Add(serviceDefinition);
        }

        return s;
    }

    private static bool AssertStatic(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetMethod.IsStatic) return true;
        throw new DslServicesNotStaticException();
    }

    private static bool AssertStatic(FieldInfo fieldInfo)
    {
        if (fieldInfo.IsStatic) return true;
        
        throw new DslServicesNotStaticException();
    }

    private static ServiceDefinition ParseDefinition(FieldInfo fieldInfo)
    {
        if (fieldInfo.FieldType != typeof(IServiceCollection))
            throw new DslServiceCollectionNotServiceCollectionException();

        var collection = (IServiceCollection)fieldInfo.GetValue(null);

        if (fieldInfo.GetCustomAttribute<OverrideServicesAttribute>() is { } overrideServicesAttribute)
            return new ServiceDefinition(collection, overrideServicesAttribute.Tag);

        return new ServiceDefinition(collection);
    }
    
    private static ServiceDefinition ParseDefinition(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetMethod.ReturnType != typeof(IServiceCollection))
            throw new DslServiceCollectionNotServiceCollectionException();

        var collection = (IServiceCollection)propertyInfo.GetValue(null);
        
        if (propertyInfo.GetCustomAttribute<OverrideServicesAttribute>() is { } overrideServicesAttribute)
            return new ServiceDefinition(collection, overrideServicesAttribute.Tag);

        return new ServiceDefinition(collection);
    }
    
    private static bool IsServiceDefinition(MemberInfo info)
    {
        var attributes = info.GetCustomAttributes()
                .Where(attribute => attribute is BaseServicesAttribute or OverrideServicesAttribute)
                .ToList()
            ;
    
        return attributes.Any();
    }

    public IEnumerable<ServiceDescriptor> GetBaseServices()
    {
        return _serviceDefinitions.SelectMany(def => def.Collection);
    }

    public IEnumerable<ServiceDescriptor> GetOverrideServices(string[] tags)
    {
        var descriptors = new List<ServiceDescriptor>();
        
        foreach (var tag in tags)
        {
            if (!_serviceOverrides.ContainsKey(tag)) throw new DslTagNotFoundException(tag, _serviceOverrides.Keys);
            
            descriptors.AddRange(_serviceOverrides[tag].Collection);
        }

        return descriptors;
    }
}