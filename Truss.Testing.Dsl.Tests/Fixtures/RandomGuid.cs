namespace Truss.Testing.Dsl.Tests.Fixtures;

public interface IGuidProvider
{
    public Guid Guid { get; }
}

public sealed class RandomGuid : IGuidProvider
{
    public Guid Guid => Guid.NewGuid();
}

public sealed class EmptyGuid : IGuidProvider
{
    public Guid Guid => Guid.Empty;
}