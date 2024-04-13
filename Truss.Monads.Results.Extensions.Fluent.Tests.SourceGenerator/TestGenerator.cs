namespace Truss.Monads.Results.Extensions.Fluent.Tests.SourceGenerator;


public sealed class TestGenerator
{
    private readonly List<string> _parameters = ["a", "b", "c", "d", "e", "f", "g"];
    private readonly int _size;

    public TestGenerator(int size)
    {
        _size = size;
    }
    
    public string Generate()
    {
        return $$"""
                 {{AndDoThen()}}
                 """;
    }

    private string GetFunctionArgument(int size, string fName)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));

        return $"({p}) => new {nameof(DummyClass)}().Do{fName}({p})";
    }
    
    private string And(int size, string fName)
    {
        return $".And({GetFunctionArgument(size, fName)})";
    }

    private string Then(int size, string fName)
    {
        return $".Then({GetFunctionArgument(size, fName)})";
    }
    
    private string Do(int size, string fName)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        return $".Do(({p}) => doCount++)";
    }

    private string AndDoThen()
    {
        var lines = new List<string>();
        

        for (var i = 1; i <= _size; i++)
        {
            lines.Add(
                $$"""
                  [Fact]
                  public void AndDoThen{{i}}()
                  {
                  var doCount = 0;
                  """
            );
            lines.AddRange(CreateBlock(i));
            lines.Add(";");
            lines.Add($"Assert.Equal(doCount, 1);");
            lines.Add("}");
        }

        return string.Join("\n", lines);
    }

    private List<string> CreateBlock(int size)
    {
        var lines = new List<string> {$"var result = new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
           lines.Add(And(i, "Sync"));
        }
        lines.Add(Do(size, "Sync"));
        lines.Add(Then(size, "Sync"));

        return lines;
    }
}