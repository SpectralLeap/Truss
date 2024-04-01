namespace Truss.Results.Extensions.Fluent.SourceGenerator.ExtensionGenerators;

public sealed class Method
{
    //public string SetName => $"{_setName}{_disambiguator ?? ""}{(ProducesResult ? "Result" : "")}{(IsAsync ? "Async" : "")}";
    public string SetName => $"{_setName}";
    public string OperationTypes => $"{InTypes}{(OutType is null ? "" : $", {OutType}")}";
    public readonly string InTypes;
    public readonly string? OutType;
    public readonly string MethodName;
    public readonly string MethodSignature;
    public readonly string MethodBody;
    public readonly string ReturnType;
    public readonly string ReturnBody;
    
    public bool ProducesResult => MethodSignature.Contains("Result<");
    public bool IsAsync => MethodSignature.Contains("Task") || ReturnType.Contains("Task");

    private readonly string? _disambiguator;
    private readonly string _setName;

    private Method(
        string setName,
        string inTypes,
        string methodName,
        string returnType,
        string methodSignature,
        string methodBody,
        string returnBody,
        string? outType,
        string? disambiguator
    )
    {
        ReturnType = returnType;
        MethodName = methodName;
        MethodSignature = methodSignature;
        MethodBody = methodBody;
        ReturnBody = returnBody;
        OutType = outType;
        _disambiguator = disambiguator;
        _setName = setName;
        InTypes = inTypes;
    }

    internal static Method Create(
        string setName,
        string inTypes,
        string methodName,
        string returnType,
        string methodSignature,
        string methodBody,
        string returnBody,
        string? outType = null,
        string? disambiguator = null
    )
    {
        return new Method(setName, inTypes, methodName, returnType, methodSignature, methodBody, returnBody, outType, disambiguator);
    }
}