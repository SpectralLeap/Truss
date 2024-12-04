using Google.Protobuf;
using Google.Protobuf.Compiler;
using Truss.Modeling.Protoc.Cqrs.Generator.Plugin;

await using var input = Console
    .OpenStandardInput();

await using var output = Console
    .OpenStandardOutput();

var availableGenerators = new Dictionary<string, TypesGeneratorBase>
{
    ["domain"] = new DomainTypesGenerator(),
    ["application"] = new ApplicationTypesGenerator(),
};

var request = CodeGeneratorRequest
    .Parser
    .ParseFrom(input);

var typeGenerations = request.Parameter.Split(",");

var activeGenerators = typeGenerations
    .Where(t => availableGenerators.TryGetValue(t, out _))
    .Select(t => availableGenerators[t])
    .ToArray();

var response = new CodeGeneratorResponse();

foreach (var file in request.ProtoFile)
{
    foreach (var generator in activeGenerators)
    {
        generator.GenerateFrom(file);
    }
}

response.File.AddRange(activeGenerators.SelectMany(g => g.GeneratedFiles));

response.WriteTo(output);