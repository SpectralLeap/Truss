namespace Truss.Testing.Dsl.Tests.Fixtures;

public sealed class OtherRegisteredDsl(RandomGuid randomGuid)
{
    public Guid Guid => randomGuid.Guid;
}