using Microsoft.CodeAnalysis.FlowAnalysis;

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
        var lines = new List<string>();
        for (int i = 1; i <= _size; i++)
        {
            lines.AddRange(AndDoThen(i, "Sync"));
            lines.AddRange(AndDoThen(i, "Async"));
            lines.AddRange(AndDoFailThen(i, "Sync"));
            lines.AddRange(AndDoFailThen(i, "Async"));
            lines.AddRange(AndThenFailThen(i, "Sync"));

            if (i > 1)
            {
                lines.AddRange(AndFailThen(i, "Sync"));
                lines.AddRange(AndFailThen(i, "Async"));
                // This does not pick up asynchronicity on the first round
                lines.AddRange(AndThenFailThen(i, "Async"));
            }
        }

        return string.Join("\n", lines);
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

    private string DoFail(int size, string fName)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));

        return $$"""
                 .Do(({{p}}) => {
                     if (1 > 0) return Result.Fail();
                     return Result.Success();
                 })
                 """;
    }

    private List<string> AndDoThen(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public {{(sync.Equals("Async") ? "async Task" : "void")}} AndDoThen{{i}}{{sync}}()
              {
              var doCount = 0;
              """
        };

        lines.AddRange(CreateBlock(i, sync));
        lines.Add(";");
        lines.Add($"Assert.Equal(doCount, 1);");
        lines.Add("}");

        return lines;
    }
    
    private List<string> AndThenFailThen(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public  {{(sync.Equals("Async") ? "async Task" : "void")}} AndThenFailThen{{i}}{{sync}}()
              {
              """
        };
        
        lines.AddRange(CreateAndThenFailBlock(i, sync));
        lines.Add(".AsResult();");
        lines.Add($"Assert.True(result.Failed);");
        lines.Add("}");
        
        return lines;
    }
    
    private List<string> AndFailThen(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public {{(sync.Equals("Async") ? "async Task" : "void")}} AndFailThen{{i}}{{sync}}()
              {
              """
        };
        
        lines.AddRange(CreateAndFailBlock(i, sync));
        lines.Add(".AsResult();");
        lines.Add($"Assert.True(result.Failed);");
        lines.Add("}");
        
        return lines;
    }
    
    private IEnumerable<string> CreateAndFailBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            if (i == size - 1)
            {
                lines.Add(AndFail(i, sync));
                continue;
            }

            lines.Add(And(i, sync));
        }
        
        lines.Add($".Then({GetFunctionArgument(size, sync)})");
            
        return lines;
    }

    private IEnumerable<string> CreateAndThenFailBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            lines.Add(And(i, sync));
        }
        lines.Add(ThenFail(size, sync));
        lines.Add(".Then(_ => \"Success\")");
            
        return lines;
    }

    private string ThenFail(int size, string fName)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        return $$"""
                 .Then(({{p}}) => {
                     if (1 > 0) return Result.Fail("fail");
                     return Result.Success(new {{nameof(DummyClass)}}().Do{{fName}}({{p}}));
                 })
                 """;
    }
    
    private string AndFail(int size, string fName)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        return $$"""
                 .And({{(fName.Equals("Async") ? "async" : "")}} ({{p}}) => {
                     if (1 > 0) return Result.Fail("fail");
                     return Result.Success({{(fName.Equals("Async") ? "await" : "")}} new {{nameof(DummyClass)}}().Do{{fName}}({{p}}));
                 })
                 """;
    }

    private List<string> AndDoFailThen(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public {{(sync.Equals("Async") ? "async Task" : "void")}} AndDoFailThen{{i}}{{sync}}()
              {
              """
        };
    
        lines.AddRange(CreateFailBlock(i, sync));
        lines.Add(".AsResult();");
        lines.Add($"Assert.True(result.Failed);");
        lines.Add("}");
    
        return lines;
    }

    private List<string> CreateBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            lines.Add(And(i, sync));
        }
        lines.Add(Do(size, sync));
        lines.Add(Then(size, sync));

        return lines;
    }
    
    private List<string> CreateFailBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            lines.Add(And(i, sync));
        }
        lines.Add(DoFail(size, sync));
        lines.Add(Then(size, sync));
    
        return lines;
    }
}