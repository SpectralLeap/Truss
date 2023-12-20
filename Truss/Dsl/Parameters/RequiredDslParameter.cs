
namespace Truss.Dsl.Parameters;

public sealed class DslRequiredParameterNotSetException(params string[] names) 
    : Exception($"Required parameters \"{string.Join(", ", names)}\" did not have a value set");

public sealed class RequiredDslParameter(string name) : DslParameter(name)
{
    public override string Value => SetValue ?? throw new DslRequiredParameterNotSetException(Name);
}