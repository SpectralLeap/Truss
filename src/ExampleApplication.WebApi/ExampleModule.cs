using Truss.AspNetCore;

namespace ExampleApplication.WebApi;

public sealed class ExampleModule : WebModule
{
    public override bool MapMessagesToEndpoints => true;
}