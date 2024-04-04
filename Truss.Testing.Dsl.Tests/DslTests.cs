using Truss.Testing.Dsl.Language;
using Truss.Testing.Dsl.Tests.Drivers;
using Truss.Testing.Dsl.Tests.Fixtures;

namespace Truss.Testing.Dsl.Tests;

public sealed class DslTests
{
    private readonly DslFixture _dslFixture = new();

    [Theory]
    [InlineData("")]
    [InlineData("name")]
    [InlineData("name:")]
    [InlineData("name: ")]
    [InlineData("name : ")]
    public void ArgumentsMustBeInNameColonValueFormat(string argument)
    {
        Assert.Throws<DslArgumentSyntaxException>(() => _dslFixture.AcceptOnlyNameAndValue(argument));
    }
    
    [Theory]
    [InlineData("name: value")]
    [InlineData("name : value")]
    [InlineData("name :value")]
    [InlineData("name  :value")]
    public void ArgumentsCanHaveWhitespaceAroundColon(string argument)
    {
        _dslFixture.AcceptOnlyNameAndValue(argument);
        
        Assert.Equal("value", _dslFixture.Value);
    }

    [Fact]
    public void AcceptsAvailableValues()
    {
        _dslFixture.AcceptOnlyNameAndValue("name: value");
    }
    
    [Fact]
    public void DoesNotAcceptNonAvailableValues()
    {
        Assert.Throws<DslValueNotAvailableException>(() => _dslFixture.AcceptOnlyNameAndValue("name: not value"));
    }
     
    [Fact]
    public void ArgumentsMustBeAvailableAsParameters()
    {
        Assert.Throws<DslArgumentNotInParameterSetException>(() => _dslFixture.AcceptOnlyNameAndValue("number: 1"));
    }

    [Theory]
    [InlineData("NAME: value")]
    [InlineData("NAME: VALUE")]
    [InlineData("name: VALUE")]
    public void CapitalizationDoesNotMatter(string argument)
    {
        _dslFixture.AcceptOnlyNameAndValue(argument);
        Assert.Equal("value", _dslFixture.Value);
    }

    [Fact]
    public void AcceptsListsAsValues()
    {
        _dslFixture.AcceptsNamesAndList("names: joe, janet, jim");
        Assert.Equal("joe, janet, jim", _dslFixture.Value);
    }
    
    [Fact]
    public void OnlyAcceptsListsWhereAllValuesAreInAvailableValues()
    {
      Assert.Throws<DslValueNotAvailableException>(() => _dslFixture.AcceptsNamesAndList("names: joe, janet, jim, not me"));
    }

    [Fact]
    public void UsesDefaultValues()
    {
        _dslFixture.UsesDefaultValueOfValue();
        Assert.Equal("value", _dslFixture.Value);
    }
    
    [Fact]
    public void CanDefaultToNull()
    {
        _dslFixture.OptionalDefaultingToNull();
        Assert.Null(_dslFixture.Value);
    }
    
    [Fact]
    public void IfRequiredNotSetThenThrows()
    {
        Assert.Throws<DslRequiredParameterNotSetException>(() => _dslFixture.RequiredParameter());
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("1293123")]
    [InlineData("93401234710707")]
    public void AllowsByPattern(string data)
    {
        _dslFixture.AcceptsIntegersByPattern($"number: {data}");
    }
    
    [Theory]
    [InlineData("n")]
    [InlineData("1 2")]
    [InlineData("1.2")]
    public void DisallowsByPattern(string data)
    {
        Assert.Throws<DslValueDoesNotMatchPattern>(() => _dslFixture.AcceptsIntegersByPattern($"number: {data}"));
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("1,2,3")]
    [InlineData("1293123, 13407, 934012347156007")]
    public void AllowsByPatternInList(string data)
    {
        _dslFixture.AcceptsIntegersByPatternInList($"numbers: {data}");
    }
     
    [Theory]
    [InlineData("n")]
    [InlineData("1,2, m")]
    [InlineData("1.2")]
    public void DisallowsByPatternInList(string data)
    {
        Assert.Throws<DslValueDoesNotMatchPattern>(() => _dslFixture.AcceptsIntegersByPatternInList($"numbers: {data}"));
    }
     
    [Fact]
    public void ThrowsIfParametersNotSetOnDslArgs()
    {
        var args = DslArgs.ForAction<RegisterUser>();
        Assert.Throws<DslArgsDoNotHaveParametersSetException>(() => args[""]);
    }
      
}