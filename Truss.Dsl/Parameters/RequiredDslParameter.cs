using Truss.Dsl.Parameters.Exceptions;

namespace Truss.Dsl.Parameters;

public sealed class RequiredDslParameter(string name) : DslParameter(name)
{
    public override string Value => SetValue ?? throw new DslRequiredParameterNotSetException(Name);
}