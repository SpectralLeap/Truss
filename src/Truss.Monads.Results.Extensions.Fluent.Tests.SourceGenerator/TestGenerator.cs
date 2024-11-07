using System.Text.RegularExpressions;

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
        for (var i = 1; i <= _size; i++)
        {
            lines.AddRange(AndDoThen(i, "Sync"));
            lines.AddRange(AndDoThen(i, "Async"));
            lines.AddRange(AndDoDispose(i, "Sync"));
            lines.AddRange(AndDoDispose(i, "Async"));
            lines.AddRange(AndDoFailThen(i, "Sync"));
            lines.AddRange(AndDoFailThen(i, "Async"));
            lines.AddRange(AndThenFailThen(i, "Sync"));
            lines.AddRange(AndThenFailThen(i, "Async"));
            lines.AddRange(AndThenFailInResultThen(i, "Sync"));
            
            lines.AddRange(ConvertFailToException(AndDoFailThen(i, "Sync")));
            lines.AddRange(ConvertFailToException(AndDoFailThen(i, "Async")));
            lines.AddRange(ConvertFailToException(AndThenFailThen(i, "Sync")));
            lines.AddRange(ConvertFailToException(AndThenFailThen(i, "Async")));
            lines.AddRange(ConvertFailToException(AndThenFailInResultThen(i, "Sync")));

            if (i != _size)
            {
                lines.AddRange(AndFailThen(i, "Sync"));
                lines.AddRange(AndFailThen(i, "Async"));
            }
            if (i > 1)
            {
                
                // This is here because async is just seen as a task
                // [Fact]
                // public  async Task AndThenFailThen1Async()
                // {
                // var result = await new DummyClass().AsResult()
                // .Then((a) => {
                //     if (1 > 0) return Result.Fail("fail");
                //     return Result.Success(new DummyClass().DoAsync(a));
                // })
                // .Then(_ => "Success")
                // .AsResult();
                // Assert.True(result.Failed);
                // }
                lines.AddRange(AndThenFailInResultThen(i, "Async"));
                
            }
        }

        return string.Join("\n", lines);
    }

    private string GetFunctionArgument(int size, string sync)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));

        return $"({p}) => new {nameof(DummyClass)}().Do{sync}({p})";
    }
    
    private string And(int size, string sync)
    {
        return $".And({GetFunctionArgument(size, sync)})";
    }

    private string Then(int size, string sync)
    {
        return $".Then({GetFunctionArgument(size, sync)})";
    }
    
    private string Do(int size, string sync)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        if (sync == "Sync")
            return $".Do(({p}) => doCount++)";

        return 
            $$"""
             .Do(async ({{p}}) => {
                await Task.Delay(0);
                doCount++;     
             })
             .Do(async ({{p}}) => {
                await Task.Delay(0);
                return Result.Success();
             })
             """;
    }

    private string DoFail(int size, string sync)
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
              """,
        };

        lines.AddRange(CreateBlock(i, sync));
        lines.Add(";");
        lines.Add("Assert.Equal(doCount, 1);");
        lines.Add("}");

        return lines;
    }
    
    private List<string> AndDoDispose(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public {{(sync.Equals("Async") ? "async Task" : "void")}} AndDoDispose{{i}}{{sync}}()
              {
              var doCount = 0;
              """,
        };
    
        lines.AddRange(CreateAndDoBlock(i, sync));
        lines.Add(";");
        
        switch (sync)
        {
            case "Sync":
                lines.Add("result.Dispose();");
                lines.Add("result.Dispose();");
                break;
            case "Async":
                lines.Add("await result.DisposeAsync();");
                break;
        }

        lines.Add("Assert.Equal(doCount, 1);");
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
              """,
        };
        
        lines.AddRange(CreateAndThenFailBlock(i, sync));
        lines.Add(".AsResult();");
        lines.Add($"Assert.True(result.Failed);");
        lines.Add("}");
        
        return lines;
    }
    
    private List<string> AndThenFailInResultThen(int i, string sync)
    {
        var lines = new List<string>
        {
            $$"""
              [Fact]
              public  {{(sync.Equals("Async") ? "async Task" : "void")}} AndThenFailInResultThen{{i}}{{sync}}()
              {
              """,
        };
         
        lines.AddRange(CreateAndThenFailInResultBlock(i, sync));
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
              """,
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
        for (var i = 1; i <= size; i++)
        {
            if (i == size)
            {
                lines.Add(AndFail(i, sync));
                continue;
            }

            lines.Add(And(i, sync));
        }
        
        lines.Add($".Then({GetFunctionArgument(size+1, sync)})");
            
        return lines;
    }

    private IEnumerable<string> CreateAndThenFailInResultBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            lines.Add(And(i, sync));
        }
        lines.Add(ThenFailInResult(size, sync));
        lines.Add(".Then(_ => \"Success\")");
            
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


    private string ThenFailInResult(int size, string sync)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        return $$"""
                 .Then(({{p}}) => {
                     if (1 > 0) return Result.Fail("fail");
                     return Result.Success(new {{nameof(DummyClass)}}().Do{{sync}}({{p}}));
                 })
                 """;
    }
    
    private string ThenFail(int size, string sync)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
            
        return $$"""
                 .Then(({{p}}) => {
                     if (1 > 0) throw new Exception("fail");
                     return new {{nameof(DummyClass)}}().Do{{sync}}({{p}});
                 })
                 """;
    }
     
    private string AndFail(int size, string sync)
    {
        var p = string.Join(", ", _parameters.GetRange(0, size));
        
        return $$"""
                 .And({{(sync.Equals("Async") ? "async" : "")}} ({{p}}) => {
                     if (1 > 0) return Result.Fail("fail");
                     return Result.Success({{(sync.Equals("Async") ? "await" : "")}} new {{nameof(DummyClass)}}().Do{{sync}}({{p}}));
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
              """,
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
    
    private List<string> CreateAndDoBlock(int size, string sync)
    {
        var lines = new List<string> {$"var result = {(sync.Equals("Async") ? "await" : "")} new {nameof(DummyClass)}().AsResult()"};
        for (var i = 1; i < size; i++)
        {
            lines.Add(And(i, sync));
        }
        lines.Add(Do(size, sync));
 
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

    private List<string> ConvertFailToException(List<string> lines)
    {
        var failureResultRegex = new Regex(@"return\s+Result.Fail\(.*\)");
        var retLines = new List<string>();

        retLines.Add(lines[0].Replace("Fail", "Exception"));
        
        foreach (var line in lines.Skip(1))
        {
            retLines.Add(failureResultRegex.Replace(line, "throw new Exception(\"Bad\")"));
        }

        return retLines;
    }
}