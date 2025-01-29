using Truss.AspNetCore.Endpoints;

namespace ExampleApplication.Module2;

public sealed class Module2 : EndpointModule
{
    public override string Name => "QueryModule";
}