using System.Collections.Generic;

namespace Truss.Monads.Results.Extensions.Fluent.SourceGenerator.MethodSets;

public sealed class DoMethodSet : IMethodSet
{
    private readonly TypingContext _tc;

    public DoMethodSet(TypingContext tc)
    {
        _tc = tc;
    }
    
    public Method[] GetMethods()
    {
        var methods = new List<Method>();

        methods.Add(Method.Create(
            setName: "Do",
            inTypes: _tc.InTypes,
            methodName: "action",
            methodSignature: $"Action<{_tc.InTypes}>",
            methodBody: $"action({_tc.PriorSuccessValues()});",
            returnType: $"{_tc.InTypes}",
            returnBody: $"({_tc.PriorSuccessValues()})"
        ));
        
        methods.Add(Method.Create(
            setName: "Do",
            inTypes: _tc.InTypes,
            methodName: "f",
            methodSignature: $"Func<{_tc.InTypes}, Result<Nil>>",
            methodBody: $"""
                         var value = f({_tc.PriorSuccessValues()});
                         """,
            returnType: $"{_tc.InTypes}",
            returnBody: $"({_tc.PriorSuccessValues()})"
        ));
         
        methods.Add(Method.Create(
            setName: "Do",
            inTypes: _tc.InTypes,
            methodName: "task",
            methodSignature: $"Func<{_tc.InTypes}, Task>",
            methodBody: $"await task({_tc.PriorSuccessValues()});",
            returnType: $"{_tc.InTypes}",
            returnBody: $"({_tc.PriorSuccessValues()})"
        ));
        
        methods.Add(Method.Create(
            setName: "Do",
            inTypes: _tc.InTypes,
            methodName: "task",
            methodSignature: $"Func<{_tc.InTypes}, Task<Result<Nil>>>",
            methodBody: $"""
                         var value = await task({_tc.PriorSuccessValues()});
                         """,
             
            returnType: $"{_tc.InTypes}",
            returnBody: $"({_tc.PriorSuccessValues()})"
        ));
         
        return methods.ToArray();
    }
}