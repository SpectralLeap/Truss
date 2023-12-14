using Truss.Core;
using Truss.Dsl.Parameters;

namespace Truss.Dsl.Tests;

public sealed class DslLayer
{
    public string? Value { get; private set; }
    
    public DslLayer AcceptOnlyNameAndValue(params string[] args)
    {
        var parameters = DslParameters.From(
            args,
            parameters: DslParameter.Optional("name")
                .SetAvailableValues("value")
        );

        Value = parameters["name"];

        return this;
    }

    public DslLayer AcceptsNamesAndList(params string[] args)
    {
        var parameters = DslParameters.From(
            args,
            parameters: DslParameter.Optional("names")
                .AsList()
                .SetAvailableValues("joe", "jim", "janet")
        );
        
        Value = parameters["names"];

        return this;
    }

    public DslLayer UsesDefaultValueOfValue(params string[] args)
    {
        var parameters = DslParameters.From(
            args,
            parameters: DslParameter.Optional("name")
                .SetDefault("value")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslLayer OptionalDefaultingToNull(params string[] args)
    {
        var parameters = DslParameters.From(
            args,
            parameters: DslParameter.Optional("name")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslLayer RequiredParameter(params string[] args)
    {
        var parameters = DslParameters.From(
            args,
            parameters: DslParameter.Required("name")
        );

        Value = parameters["name"];
        
        return this;
    }
}
