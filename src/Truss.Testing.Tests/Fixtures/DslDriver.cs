using Truss.Testing.Dsl;
using Truss.Testing.Tests.Drivers;

namespace Truss.Testing.Tests.Fixtures;

public sealed class DslDriver
{
    public string? Value { get; private set; }
    
    public DslDriver AcceptOnlyNameAndValue(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Optional("name")
                .SetAvailableValues("value")
        );

        Value = parameters["name"];

        return this;
    }

    public DslDriver AcceptsNamesAndList(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Optional("names")
                .AsList()
                .SetAvailableValues("joe", "jim", "janet")
        );
        
        Value = parameters["names"];

        return this;
    }

    public DslDriver UsesDefaultValueOfValue(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Optional("name")
                .SetDefault("value")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslDriver OptionalDefaultingToNull(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Optional("name")
        );

        Value = parameters["name"];
        
        return this;
    }

    public DslDriver RequiredParameter(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Required("name")
        );

        Value = parameters["number"];
        
        return this;
    }

    public DslDriver AcceptsIntegersByPattern(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Required("number")
                .SetPattern(@"\d+")
        );
        
        Value = parameters["number"];
                
        return this;
    }

    public void AcceptsIntegersByPatternInList(params string[] args)
    {
        var parameters = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
            args,
            parameters: DslParameter.Required("numbers")
                .AsList()
                .SetPattern(@"\d+")
        );
                
        Value = parameters["numbers"];
    }

    public void ThrowsIfNoParameters(params string[] args)
    {
        var parameters = DslArgs.ForAction<RegisterUserDriver>();

        Value = parameters["numbers"];
    }
}
