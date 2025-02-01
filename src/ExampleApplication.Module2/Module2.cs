using Truss.AspNetCore;

namespace ExampleApplication.Module2;

public sealed class Module2 : WebModule
{
    public override string Name => "QueryModule";
    public override bool MapMessagesToEndpoints => true;
}