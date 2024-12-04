using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;

namespace Truss.Modeling.Protoc.Cqrs.Generator.Plugin;

internal abstract class TypesGeneratorBase
{
    public IReadOnlyCollection<CodeGeneratorResponse.Types.File> GeneratedFiles => _generatedFiles;
    private readonly List<CodeGeneratorResponse.Types.File> _generatedFiles = [];

    protected abstract CodeGeneratorResponse.Types.File[] FileGenerator(FileDescriptorProto file);

    public void GenerateFrom(FileDescriptorProto file)
    {
        _generatedFiles.AddRange(FileGenerator(file));
    }
}