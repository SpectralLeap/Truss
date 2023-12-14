using Truss.Dsl;
using Truss.Dsl.Parameters.Exceptions;

namespace Truss.Dsl.Tests;

public class TestClass
{
    private readonly DslLayer _dslLayer = new();

    [Theory]
    [InlineData("")]
    [InlineData("name")]
    [InlineData("name:")]
    [InlineData("name: ")]
    [InlineData("name : ")]
    public void ArgumentsMustBeInNameColonValueFormat(string argument)
    {
        Assert.Throws<DslArgumentSyntaxException>(() => _dslLayer.AcceptOnlyNameAndValue(argument));
    }
    
    [Theory]
    [InlineData("name: value")]
    [InlineData("name : value")]
    [InlineData("name :value")]
    [InlineData("name  :value")]
    public void ArgumentsCanHaveWhitespaceAroundColon(string argument)
    {
        _dslLayer.AcceptOnlyNameAndValue(argument);
        
        Assert.Equal("value", _dslLayer.Value);
    }

    [Fact]
    public void AcceptsAvailableValues()
    {
        _dslLayer.AcceptOnlyNameAndValue("name: value");
    }
    
    [Fact]
    public void DoesNotAcceptNonAvailableValues()
    {
        Assert.Throws<DslValueNotAvailableException>(() => _dslLayer.AcceptOnlyNameAndValue("name: not value"));
    }
     
    [Fact]
    public void ArgumentsMustBeAvailableAsParameters()
    {
        Assert.Throws<DslArgumentNotInParameterSetException>(() => _dslLayer.AcceptOnlyNameAndValue("number: 1"));
    }

    [Theory]
    [InlineData("NAME: value")]
    [InlineData("NAME: VALUE")]
    [InlineData("name: VALUE")]
    public void CapitalizationDoesNotMatter(string argument)
    {
        _dslLayer.AcceptOnlyNameAndValue(argument);
        Assert.Equal("value", _dslLayer.Value);
    }

    [Fact]
    public void AcceptsListsAsValues()
    {
        _dslLayer.AcceptsNamesAndList("names: joe, janet, jim");
        Assert.Equal("joe, janet, jim", _dslLayer.Value);
    }
    
    [Fact]
    public void OnlyAcceptsListsWhereAllValuesAreInAvailableValues()
    {
      Assert.Throws<DslValueNotAvailableException>(() => _dslLayer.AcceptsNamesAndList("names: joe, janet, jim, not me"));
    }

    [Fact]
    public void UsesDefaultValues()
    {
        _dslLayer.UsesDefaultValueOfValue();
        Assert.Equal("value", _dslLayer.Value);
    }
    
    [Fact]
    public void CanDefaultToNull()
    {
        _dslLayer.OptionalDefaultingToNull();
        Assert.Null(_dslLayer.Value);
    }
    
    [Fact]
    public void IfRequiredNotSetThenThrows()
    {
        Assert.Throws<DslRequiredParameterNotSetException>(() => _dslLayer.RequiredParameter());
    }
}