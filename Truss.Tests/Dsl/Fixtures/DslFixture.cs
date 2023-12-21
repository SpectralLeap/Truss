using Truss.Dsl.Arguments;
using Truss.Dsl.Parameters;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class DslFixture
{
    public string? Value { get; private set; }
    
    public DslFixture AcceptOnlyNameAndValue(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Optional("name")
                .SetAvailableValues("value")
        );

        Value = parameters["name"];

        return this;
    }

    public DslFixture AcceptsNamesAndList(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Optional("names")
                .AsList()
                .SetAvailableValues("joe", "jim", "janet")
        );
        
        Value = parameters["names"];

        return this;
    }

    public DslFixture UsesDefaultValueOfValue(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Optional("name")
                .SetDefault("value")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslFixture OptionalDefaultingToNull(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Optional("name")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslFixture RequiredParameter(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Required("name")
        );

        Value = parameters["number"];
        
        return this;
    }

    public DslFixture AcceptsIntegersByPattern(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Required("number")
                .SetPattern(@"\d+")
        );
        
        Value = parameters["number"];
                
        return this;
    }

    public void AcceptsIntegersByPatternInList(params string[] args)
    {
        var parameters = DslArgs.From(
            args,
            parameters: DslParameter.Required("numbers")
                .AsList()
                .SetPattern(@"\d+")
        );
                
        Value = parameters["numbers"];
                        
    }
}
