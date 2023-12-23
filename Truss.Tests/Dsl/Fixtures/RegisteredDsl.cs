using Microsoft.Extensions.DependencyInjection;
using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public sealed class RegisteredDsl(
    RandomGuid randomGuid,
    IUserInfo userInfo
)
{
    [BaseServices]
     private static readonly IServiceCollection BaseServices = new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
         ;
        
    [OverrideServices("admin")]
    private static readonly IServiceCollection AdminServices = new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid Guid => randomGuid.Guid;

    public bool IsAdmin => userInfo.IsAdmin;
}

public sealed class RegisteredDslWithPropertiesDefined(
    RandomGuid randomGuid,
    IUserInfo userInfo
)
{
    [BaseServices]
     private static IServiceCollection BaseServices => new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
         ;
        
    [OverrideServices("admin")]
    private static IServiceCollection AdminServices => new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid Guid => randomGuid.Guid;

    public bool IsAdmin => userInfo.IsAdmin;
}

public sealed class RegisteredDslWithNonStaticDefinitions(
    RandomGuid randomGuid,
    IUserInfo userInfo
)
{
    [BaseServices]
     private IServiceCollection BaseServices => new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
         ;
        
    [OverrideServices("admin")]
    private IServiceCollection AdminServices => new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid Guid => randomGuid.Guid;

    public bool IsAdmin => userInfo.IsAdmin;
}