using Truss.Testing.Dsl;
using Truss.Testing.Tests.Drivers;

namespace Truss.Testing.Tests.Fixtures;

public sealed class DslDomainSpecificLanguage
{
    public string? Value { get; private set; }
    
    public DslDomainSpecificLanguage AcceptOnlyNameAndValue(params string[] args)
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

    public DslDomainSpecificLanguage AcceptsNamesAndList(params string[] args)
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

    public DslDomainSpecificLanguage UsesDefaultValueOfValue(params string[] args)
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

    public DslDomainSpecificLanguage OptionalDefaultingToNull(params string[] args)
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

    public DslDomainSpecificLanguage RequiredParameter(params string[] args)
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

    public DslDomainSpecificLanguage AcceptsIntegersByPattern(params string[] args)
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
