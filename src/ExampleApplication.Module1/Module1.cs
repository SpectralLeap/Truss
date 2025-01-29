using Truss.AspNetCore;

namespace ExampleApplication.Module1;

public sealed class Module1 : WebModule
{
    public override string Name => "CommandModule";
    public override bool AutoMapMessagesAsEndpoints => true;
}