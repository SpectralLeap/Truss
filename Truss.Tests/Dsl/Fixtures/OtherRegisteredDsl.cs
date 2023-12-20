using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class OtherRegisteredDsl(RandomGuid randomGuid, IIntegrationBus integrationBus) : DomainDsl(integrationBus)
{
    public Guid Guid => randomGuid.Guid;
}