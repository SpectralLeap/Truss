using System;
using System.Collections.Generic;

namespace Truss.Results.Extensions.Fluent.Tests.SourceGenerator;

public sealed class TestGenerator
{
    private readonly int _size;

    public TestGenerator(int size)
    {
        _size = size;
    }
    
    public string Generate()
    {
        return 
            $$"""
             [Fact]
             public void AndThen() 
             {
             {{AndThenTest()}}
             }
             """;
    }

    private string AndThenTest()
    {
        var lines = new List<string>
        {
            "3.AsResult()"
        };

        for (var i = 1; i <= _size - 1; i++)
        {
            lines.AddRange(CreateAndBlock(i));
        }

        lines.Add(";");
        return string.Join("\n", lines);
    }

    private List<string> CreateAndBlock(int size)
    {
        var lines = new List<string> {".And(a => a +1)"};
        for (var i = 2; i < size; i++)
        {
            var arguments = new List<string>{ "a" };
 
            for (var j = 2; j <= size; j++)
            {
                arguments.Add("_");
            }
            
            lines.Add($".And(({string.Join(", ", arguments)}) => a + 1)");
        }

        return lines;
    }
}

public static class IntExtensions
{
    public static void TimesDo(this int number, Action action)
    {
        for (int i = 1; i <= number; i++)
        {
            action();
        }
    }
    
    public static void TimesDo(this int number, Action<int> action)
    {
        for (int i = 0; i < number; i++)
        {
            action(i);
        }
    }
}