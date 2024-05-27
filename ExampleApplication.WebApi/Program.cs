using ExampleApplication.WebApi.Services;
using Truss.Modeling.Application.Cqrs.Commands;
using Truss.Modeling.Installation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services
    .AddTruss(c => 
        c.InstallModule<Module>());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", async (ICommandBus commandBus) =>
    {
        var x = await commandBus.SendCommand<GreetCommand, GreetResult>(new GreetCommand
        {
            Subject = Guid.NewGuid().ToString()
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