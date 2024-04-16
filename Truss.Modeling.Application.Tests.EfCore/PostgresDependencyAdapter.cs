namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class PostgresDependencyAdapter
{
    public string ConnectionString { get; }

    public PostgresDependencyAdapter(string connectionString)
    {
        ConnectionString = connectionString;
    }
}