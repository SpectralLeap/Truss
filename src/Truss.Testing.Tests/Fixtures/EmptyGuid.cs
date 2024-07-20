namespace Truss.Testing.Tests.Fixtures;

public sealed class EmptyGuid : IGuidProvider
{
    public Guid Guid => Guid.Empty;
}