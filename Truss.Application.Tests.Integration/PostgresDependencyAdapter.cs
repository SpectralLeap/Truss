namespace Truss.Application.Tests.Integration;

public sealed class PostgresDependencyAdapter
{
    public string ConnectionString { get; }

    public PostgresDependencyAdapter(string connectionString)
    {
        ConnectionString = connectionString;
    }
}