using Truss.Testing.Dsl;
using Truss.Testing.Tests.Drivers;
using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests;

public sealed class DriverTests
{
    private readonly DslDriver _dslDriver = new();

    [Theory]
    [InlineData("")]
    [InlineData("name")]
    [InlineData("name:")]
    [InlineData("name: ")]
    [InlineData("name : ")]
    public void ArgumentsMustBeInNameColonValueFormat(string argument)
    {
        Assert.Throws<DslArgumentSyntaxException>(() => _dslDriver.AcceptOnlyNameAndValue(argument));
    }
    
    [Theory]
    [InlineData("name: value")]
    [InlineData("name : value")]
    [InlineData("name :value")]
    [InlineData("name  :value")]
    public void ArgumentsCanHaveWhitespaceAroundColon(string argument)
    {
        _dslDriver.AcceptOnlyNameAndValue(argument);
        
        Assert.Equal("value", _dslDriver.Value);
    }

    [Fact]
    public void AcceptsAvailableValues()
    {
        _dslDriver.AcceptOnlyNameAndValue("name: value");
    }
    
    [Fact]
    public void DoesNotAcceptNonAvailableValues()
    {
        Assert.Throws<DslValueNotAvailableException>(() => _dslDriver.AcceptOnlyNameAndValue("name: not value"));
    }
     
    [Fact]
    public void ArgumentsMustBeAvailableAsParameters()
    {
        Assert.Throws<DslArgumentNotInParameterSetException>(() => _dslDriver.AcceptOnlyNameAndValue("number: 1"));
    }

    [Theory]
    [InlineData("NAME: value")]
    [InlineData("NAME: VALUE")]
    [InlineData("name: VALUE")]
    public void CapitalizationDoesNotMatter(string argument)
    {
        _dslDriver.AcceptOnlyNameAndValue(argument);
        Assert.Equal("value", _dslDriver.Value);
    }

    [Fact]
    public void AcceptsListsAsValues()
    {
        _dslDriver.AcceptsNamesAndList("names: joe, janet, jim");
        Assert.Equal("joe, janet, jim", _dslDriver.Value);
    }
    
    [Fact]
    public void OnlyAcceptsListsWhereAllValuesAreInAvailableValues()
    {
      Assert.Throws<DslValueNotAvailableException>(() => _dslDriver.AcceptsNamesAndList("names: joe, janet, jim, not me"));
    }

    [Fact]
    public void UsesDefaultValues()
    {
        _dslDriver.UsesDefaultValueOfValue();
        Assert.Equal("value", _dslDriver.Value);
    }
    
    [Fact]
    public void CanDefaultToNull()
    {
        _dslDriver.OptionalDefaultingToNull();
        Assert.Null(_dslDriver.Value);
    }
    
    [Fact]
    public void IfRequiredNotSetThenThrows()
    {
        Assert.Throws<DslRequiredParameterNotSetException>(() => _dslDriver.RequiredParameter());
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("1293123")]
    [InlineData("93401234710707")]
    public void AllowsByPattern(string data)
    {
        _dslDriver.AcceptsIntegersByPattern($"number: {data}");
    }
    
    [Theory]
    [InlineData("n")]
    [InlineData("1 2")]
    [InlineData("1.2")]
    public void DisallowsByPattern(string data)
    {
        Assert.Throws<DslValueDoesNotMatchPattern>(() => _dslDriver.AcceptsIntegersByPattern($"number: {data}"));
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("1,2,3")]
    [InlineData("1293123, 13407, 934012347156007")]
    public void AllowsByPatternInList(string data)
    {
        _dslDriver.AcceptsIntegersByPatternInList($"numbers: {data}");
    }
     
    [Theory]
    [InlineData("n")]
    [InlineData("1,2, m")]
    [InlineData("1.2")]
    public void DisallowsByPatternInList(string data)
    {
        Assert.Throws<DslValueDoesNotMatchPattern>(() => _dslDriver.AcceptsIntegersByPatternInList($"numbers: {data}"));
    }
     
    [Fact]
    public void ThrowsIfParametersNotSetOnDslArgs()
    {
        var args = DslArgs.ForAction<RegisterUserDriver>();
        Assert.Throws<DslArgsDoNotHaveParametersSetException>(() => args[""]);
    }
      
}