namespace Truss.Monads.Results.Extensions.Fluent.Tests.SourceGenerator;

public sealed class DummyClass
{
    private string Value = "";
    
    public DummyClass DoSync(params DummyClass[] classes)
    {
        
        Value = string.Join("s", classes.Select(c => c.Value));
        
        return this;
    }
}