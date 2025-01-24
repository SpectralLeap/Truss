namespace Truss.Infrastructure.Tests.Dependencies;

public sealed class PostgresDependencyAdapter
{
    public string ConnectionString { get; }

    public PostgresDependencyAdapter(string connectionString)
    {
        ConnectionString = connectionString;
    }
}