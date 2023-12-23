using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class OtherRegisteredDsl(RandomGuid randomGuid)
{
    public Guid Guid => randomGuid.Guid;
}