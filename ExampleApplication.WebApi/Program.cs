using ExampleApplication.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGet("/heartbeat", () => "OK");

app.Run();

// Exposes the program class for testing.
//
// Preferable to InternalsVisibleTo
namespace ExampleApplication.WebApi
{
    public partial class Program {}
}