using ExampleApplication.Hosted;
using Truss;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddTruss();

var host = builder.Build();
host.Run();