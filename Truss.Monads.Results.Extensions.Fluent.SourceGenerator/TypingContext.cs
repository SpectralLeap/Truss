using System.Collections.Generic;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator;

public sealed class TypingContext
{
    public readonly string OutType = "TOut";
    public readonly string PriorResultName = "result";
    public string InTypes { get; } 
    public string InTuple { get; }

    public readonly int Size;
    
    public string PriorSuccessValues()
    {
        if (Size is 1) return $"{PriorResultName}.SuccessValue";
     
        var values = new List<string> {
            $"{PriorResultName}.SuccessValue.Item1"
        };

        for (int i = 2; i <= Size; i++)
        {
            values.Add($"{PriorResultName}.SuccessValue.Item{i}");
        }
     
        return string.Join(",\n", values); 
    }
    
    public TypingContext(int size)
    {
        var inTypeArray = new List<string>();

        for (int i = 1; i <= size; i++)
        {
            inTypeArray.Add($"TSuccess{i}");
        }
       
        InTypes = string.Join(", ", inTypeArray);
        InTuple = size > 1 ? $"({InTypes})" : InTypes;
        Size = size;
    }
    

}