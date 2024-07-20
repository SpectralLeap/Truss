namespace Truss.Testing.Tests.Fixtures;

public sealed class OtherRegisteredDsl(RandomGuid randomGuid)
{
    public Guid Guid => randomGuid.Guid;
}