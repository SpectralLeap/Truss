using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.Compiler;

using var cancellationSource = new CancellationTokenSource();

var cancellationToken = cancellationSource.Token;

await File.WriteAllTextAsync(
    "0_log.txt",
    "Creating protos",
    cancellationToken
);

await using var input = Console
    .OpenStandardInput();

await using var output = Console
    .OpenStandardOutput();

var request = CodeGeneratorRequest
    .Parser
    .ParseFrom(input);


var x = request.Parameter;

var response = new CodeGeneratorResponse();

foreach (var file in request.ProtoFile)
{
    foreach (var message in file.MessageType)
    {
        // Generate DTOs
        response.File.Add(new CodeGeneratorResponse.Types.File
        {
            Name = $"{message.Name}Dto.cs",
            Content = GenerateDto(message),
        });

        // Generate Commands
        response.File.Add(new CodeGeneratorResponse.Types.File
        {
            Name = $"{message.Name}Command.cs",
            Content = GenerateCommand(message),
        });

        // Generate Queries
        response.File.Add(new CodeGeneratorResponse.Types.File
        {
            Name = $"{message.Name}Query.cs",
            Content = GenerateQuery(message),
        });
    }
}

response.WriteTo(output);

static string GenerateDto(DescriptorProto message)
{
    var fields = string.Join('\n', message.Field.Select(f =>
        $"public {GetCSharpType(f)} {f.Name} {{ get; set; }}"));
    return $@"
            public class {message.Name}Dto
            {{
                {fields}
            }}
        ";
}

static string GenerateCommand(DescriptorProto message)
{
    return $@"
            public class {message.Name}Command
            {{
                // Add command-specific logic here
            }}
        ";
}

static string GenerateQuery(DescriptorProto message)
{
    return $@"
            public class {message.Name}Query
            {{
                // Add query-specific logic here
            }}
        ";
}

static string GetCSharpType(FieldDescriptorProto field)
{
    return field.Type switch
    {
        FieldDescriptorProto.Types.Type.Int32 => "int",
        FieldDescriptorProto.Types.Type.String => "string",
        _ => "object",
    };
}