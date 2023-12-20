using Microsoft.Extensions.DependencyInjection;
using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class RegisteredDslAdminOverrideSet() 
    : DomainDslOverrideSet<RegisteredDsl>("admin",
        new ServiceCollection()
            .AddSingleton<IUserInfo, AdminInfo>()
        )
{
    
}

public sealed class RegisteredDsl(
    IIntegrationBus integrationBus,
    RandomGuid randomGuid,
    IUserInfo userInfo
) : DomainDsl(integrationBus)
{
    public Guid Guid => randomGuid.Guid;

    public bool IsAdmin => userInfo.IsAdmin;
}