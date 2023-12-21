using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class RegisteredDsl(
    IIntegrationBus integrationBus,
    RandomGuid randomGuid,
    IUserInfo userInfo
) : DomainDsl(integrationBus)
{
    public Guid Guid => randomGuid.Guid;

    public bool IsAdmin => userInfo.IsAdmin;
}