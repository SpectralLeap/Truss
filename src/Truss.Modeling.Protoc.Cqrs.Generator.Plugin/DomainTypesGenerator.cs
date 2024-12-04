using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;

namespace Truss.Modeling.Protoc.Cqrs.Generator.Plugin;

internal sealed class DomainTypesGenerator : TypesGeneratorBase
{
    protected override CodeGeneratorResponse.Types.File[] FileGenerator(FileDescriptorProto file)
    {
        return file.MessageType
            .Where(m =>
                !m.Name.EndsWith("Request")
                || !m.Name.EndsWith("Response")

            ).Select(m =>
                new {
                    m.Name,
                    Content = $$"""
                                public sealed class {{m.Name}}Dto
                                {
                                }
                                """
                }
            ).Select(x =>
                new CodeGeneratorResponse.Types.File
                {
                    Name = x.Name,
                    Content = x.Content,
                }
            ).ToArray();
    }
}