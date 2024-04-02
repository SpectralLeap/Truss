using System.Collections.Generic;

namespace Truss.Results.Extensions.Fluent.SourceGenerator.MethodSets;
public sealed class AndMethodSet : IMethodSet
{
    private readonly TypingContext _tc;

    public AndMethodSet(TypingContext tc)
    {
        _tc = tc;
    }
    
    public Method[] GetMethods()
    {
        var methods = new List<Method>();

        methods.Add(Method.Create(
            setName: "And",
            inTypes: _tc.InTypes,
            outType: _tc.OutType,
            methodName: "map",
            methodSignature: $"Func<{_tc.InTypes}, {_tc.OutType}>",
            methodBody: $"var value = map({_tc.PriorSuccessValues()});",
            returnType: $"{_tc.InTypes}, {_tc.OutType}",
            returnBody: $"({_tc.PriorSuccessValues()}, value)"
        ));

        methods.Add(Method.Create(
            setName: "And",
            inTypes: _tc.InTypes,
            outType: _tc.OutType,
            methodName: "map",
            methodSignature: $"Func<{_tc.InTypes}, Result<{_tc.OutType}>>",
            methodBody: $"var value = map({_tc.PriorSuccessValues()});",
            returnType: $"{_tc.InTypes}, {_tc.OutType}",
            returnBody: $"({_tc.PriorSuccessValues()}, value.SuccessValue)"
        ));

        methods.Add(Method.Create(
            setName: "And",
            inTypes: _tc.InTypes,
            outType: _tc.OutType,
            methodName: "map",
            methodSignature: $"Func<{_tc.InTypes}, Task<Result<{_tc.OutType}>>>",
            methodBody: $"var value = await map({_tc.PriorSuccessValues()}).ConfigureAwait(false);",
            returnType: $"{_tc.InTypes}, {_tc.OutType}",
            returnBody: $"({_tc.PriorSuccessValues()}, value.SuccessValue)"
        ));

        methods.Add(Method.Create(
            setName: "And",
            inTypes: _tc.InTypes,
            outType: _tc.OutType,
            methodName: "map",
            methodSignature: $"Func<{_tc.InTypes}, Task<{_tc.OutType}>>",
            methodBody: $"var value = await map({_tc.PriorSuccessValues()}).ConfigureAwait(false);",
            returnType: $"{_tc.InTypes}, {_tc.OutType}",
            returnBody: $"({_tc.PriorSuccessValues()}, value)"
        ));

        return methods.ToArray();
    }
}