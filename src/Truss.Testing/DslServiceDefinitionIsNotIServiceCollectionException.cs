using System.Reflection;

namespace Truss.Testing;

/// <summary>
/// Represents an exception that is thrown when a DSL Collection is not of type IServiceCollection.
/// </summary>
public sealed class DslServiceDefinitionIsNotIServiceCollectionException(MemberInfo info) 
    : Exception($"{info.Name} is not an IServiceCollection. All service definitions must be defined as IServiceCollection");