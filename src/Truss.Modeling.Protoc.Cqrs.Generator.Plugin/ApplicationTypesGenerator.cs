using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;

namespace Truss.Modeling.Protoc.Cqrs.Generator.Plugin;

internal sealed class ApplicationTypesGenerator : TypesGeneratorBase
{
    protected override CodeGeneratorResponse.Types.File[] FileGenerator(FileDescriptorProto file)
    {
        return file.MessageType
            .Where(m =>
                m.Name.EndsWith("Request")
                || m.Name.EndsWith("Response")
            ).Select(GetQueryOrCommand)
            .ToArray();
    }

    private CodeGeneratorResponse.Types.File GetQueryOrCommand(DescriptorProto descriptorProto)
    {
        if (descriptorProto.Name.StartsWith("Get"))
        {
            return GetQuery(descriptorProto);
        }

        return GetCommand(descriptorProto);
    }

    private CodeGeneratorResponse.Types.File GetCommand(DescriptorProto descriptorProto)
    {
        var name = descriptorProto.Name.Replace("Request", "Command");

        var content =
            $$"""
              public sealed class {{name}}
              {
              }
              """;

        return new CodeGeneratorResponse.Types.File
        {
            Name = $"{name}.g.cs",
            Content = content
        };
    }

    private CodeGeneratorResponse.Types.File GetQuery(DescriptorProto descriptorProto)
    {
        var name = descriptorProto.Name.Replace("Request", "Query");

        var content =
            $$"""
              public sealed class {{name}}
              {
              }
              """;

        return new CodeGeneratorResponse.Types.File
        {
            Name = $"{name}.g.cs",
            Content = content
        };
    }
}