using ExampleApplication.Module1;
using ExampleApplication.Module2;
using ExampleApplication.WebApi;
using ExampleApplication.WebApi.Services;
using Truss.AspNetCore;
using Truss.Modeling.Application.Cqrs.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.UseTruss(c =>
        c.InstallModule<ExampleModule>()
            .InstallModule<Module1>()
            .InstallModule<Module2>()
);

builder.Services.AddGrpc();

var app = builder.Build();

app.UseTruss();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", async (ICommandBus commandBus) =>
    {
        var x = await commandBus.SendCommand(new GreetCommand
        {
            Subject = Guid.NewGuid().ToString(),
        });

        return x.SuccessValue.Greeting;
    }
);

app.MapGet("/heartbeat", () => "OK");

app.Run();


// Exposes the program class for testing.
//
// Preferable to InternalsVisibleTo
namespace ExampleApplication.WebApi
{
    public sealed class Program;
}