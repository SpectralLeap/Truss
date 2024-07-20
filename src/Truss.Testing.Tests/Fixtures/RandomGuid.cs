namespace Truss.Testing.Tests.Fixtures;

public sealed class RandomGuid : IGuidProvider
{
    public Guid Guid => Guid.NewGuid();
}