using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl;
using Truss.Testing.Dsl.Services;

namespace Truss.Application.Tests.Integration;

public sealed class AutoShopDsl : Dsl
{
    private static IContainer _postgresContainer;
    
    [BaseServices]
    public static IServiceCollection Services {
        get
        {
            var username = $"test_user_{Guid.NewGuid()}";
            var password = Guid.NewGuid().ToString();

            _postgresContainer = new ContainerBuilder()
                    .WithImage("postgres:latest")
                    .WithEnvironment(new Dictionary<string, string>
                    {
                        { "POSTGRES_USER", username },
                        { "POSTGRES_PASSWORD", password }
                    })
                    .WithPortBinding(5432, true)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                    .Build()
                ;

            var postgresPort = _postgresContainer.GetMappedPublicPort(5432);
            var connectionString = $"Server=localhost;Port={postgresPort};Database=Autos;Username={username};Password={password}";
        }
    }

    public AutoShopDsl()
    {

    }
}


public sealed class EntityFrameworkTests : IClassFixture<DslFactory>
{
    private readonly DslFactory _factory;

    public EntityFrameworkTests(DslFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public void Test1()
    {
        var dsl = _factory.GetDsl<AutoShopDsl>();
        
    }
}