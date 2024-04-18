
namespace Truss.Testing.Dsl;

public sealed class RequiredDslParameter(string name) : DslParameter(name)
{
    public override string Value => SetValue ?? throw new DslRequiredParameterNotSetException(Name);
}