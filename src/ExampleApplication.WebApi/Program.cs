using ExampleApplication.Module1;
using ExampleApplication.Module2;
using ExampleApplication.WebApi;
using ExampleApplication.WebApi.Services;
using Microsoft.OpenApi.Models;
using Truss.AspNetCore;
using Truss.Infrastructure.FluentValidation;
using Truss.Infrastructure.OpenTelemetry;
using Truss.Infrastructure.Serilog;
using Truss.Modeling.Application.Cqrs.Commands;

const string title = "Auth.Api";
const string version = "v1";

var builder = WebApplication.CreateBuilder(args);

builder.UseTruss(c => c
    .InstallModule<ExampleModule>()
    .InstallModule<Module1>()
    .InstallModule<Module2>()
    .AddFluentValidation()
    .AddSerilog()
    .AddOpenTelemetry()
);

builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc(version, new OpenApiInfo
        {
            Title = title,
            Version = version
        });
    });


var app = builder.Build();

app.UseTruss();

app.UseSwagger();
app.UseSwaggerUI();

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