using System.Diagnostics;

namespace Truss.Modeling.Protoc.Cqrs.Gernator.Plugin.Tests;

public sealed class ProtocGenerationOutput
{
    public IReadOnlyCollection<string> Files => _files;
    private readonly List<string> _files = [];

    public ProtocGenerationOutput ReadFrom(string outputDir)
    {
        _files.AddRange(Directory.GetFiles(outputDir)
            .Select(File.ReadAllText));

        return this;
    }
}

public sealed class ProtocFileBuilder
{
    private readonly List<string> _messages = [];
    private readonly List<string> _arguments = [];

    public ProtocFileBuilder WithGenerateApplicationTypesArgument()
    {
        _arguments.Add("application");
        return this;
    }

    public ProtocFileBuilder WithDomainGenerationArgument()
    {
        _arguments.Add("domain");

        return this;
    }

    public ProtocFileBuilder WithMessage(string message)
    {
        _messages.Add(message);

        return this;
    }

    public async Task<ProtocGenerationOutput> Run()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Directory.CreateDirectory(tempDir);

        var protoFile = Path.Combine(tempDir, "test.proto");

        await File.WriteAllTextAsync(
            protoFile,
            $@"
                    syntax = ""proto3"";
                    package test;

            {string.Join("\n", _messages)}
        ");

        var pluginPath = "Truss.Modeling.Protoc.Cqrs.Generator.Plugin";

        var outputDir = Path.Combine(tempDir, "output");
        Directory.CreateDirectory(outputDir);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "protoc",
                Arguments = $"--plugin=protoc-gen-custom=\"{pluginPath}\" " +
                            $"--custom_out=\"{outputDir}\" " +
                            $"--custom_opt=\"{string.Join(",", _arguments)}\" " +
                            $"--proto_path=\"{tempDir}\" test.proto",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        await process.WaitForExitAsync();

        return new ProtocGenerationOutput().ReadFrom(outputDir);
    }

}

public sealed class UnitTest1
{
    [Fact]
    public async Task ProducesOutput()
    {
        var output = await new ProtocFileBuilder()
            .WithDomainGenerationArgument()
            .WithMessage(
            @" 
                 message TestMessage {
                     string name = 1;
                     int32 id = 2;
                 }
             ").Run();

        Assert.True(output.Files.Count > 0, "No files were generated.");
    }

    [Fact]
    public async Task WhenGivenNormalMessage_ProducesDtoClass()
    {
        var output = await new ProtocFileBuilder()
            .WithDomainGenerationArgument()
            .WithMessage(
                @"
                    message TestMessage {}
                "
            ).Run();

        Assert.Contains("public sealed class TestMessageDto\n", output.Files.First());
    }

    [Fact]
    public async Task WhenGivenRequest_ProducesCommand()
    {
         var output = await new ProtocFileBuilder()
             .WithGenerateApplicationTypesArgument()
             .WithMessage(
                 @"
                     message TestRequest {}
                 "
             ).Run();

         Assert.Contains("public sealed class TestCommand\n", output.Files.First());
    }

    [Fact]
    public async Task WhenGivenGetRequest_ProducesQuery()
    {
        var output = await new ProtocFileBuilder()
            .WithGenerateApplicationTypesArgument()
            .WithMessage(
                @"
                         message GetTestRequest {}
                     "
            ).Run();

        Assert.Contains("public sealed class GetTestQuery\n", output.Files.First());
    }
}
