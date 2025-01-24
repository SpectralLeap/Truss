using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Npgsql;
using Truss.Testing.SharedDependencies;

namespace Truss.Infrastructure.Tests.Dependencies;

public sealed class PostgresDatabaseSharedDependency 
    : ISharedDependency
{
    public static string? ConnectionString;
    
    [SharedDependencyAdapter]
    // ReSharper disable once NotAccessedField.Global
    public PostgresDependencyAdapter? DependencyAdapter;
    
    private IContainer? _postgresContainer;

    public async Task StartAsync()
    {
        var username = $"test_user_{Guid.NewGuid()}";
        var password = Guid.NewGuid().ToString();

        _postgresContainer = new ContainerBuilder()
                .WithImage("postgres:latest")
                .WithEnvironment(new Dictionary<string, string>
                {
                    { "POSTGRES_USER", username },
                    { "POSTGRES_PASSWORD", password },
                })
                .WithPortBinding(5432, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build()
            ;

        await _postgresContainer.StartAsync();
        
        var postgresPort = _postgresContainer.GetMappedPublicPort(5432);

        ConnectionString =
            $"Server=localhost;Port={postgresPort};Username={username};Password={password}";

        await CreateDatabaseAsync(ConnectionString, "testing");


        DependencyAdapter = new PostgresDependencyAdapter(
            ConnectionString
        );
    }

    private async Task CreateDatabaseAsync(string connectionString, string databaseName)
    {
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE \"{databaseName}\"";
        await command.ExecuteNonQueryAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
    }

}