using Microsoft.Extensions.DependencyInjection;
using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

public class RegisteredDsl(IServiceProvider provider)
{
    
    [BaseServices]
    private static readonly IServiceCollection BaseServices = new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
        ;
        
    [ServiceOverride("admin")]
    private static readonly IServiceCollection AdminServices = new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid ProviderGuid => provider.GetService<RandomGuid>()!.Guid;

    public Guid TypeGuid => Guid.NewGuid();
    
    public bool IsAdmin => provider.GetService<IUserInfo>()!.IsAdmin;
}

public class RegisteredDslWithPropertiesDefined(IServiceProvider provider)
{
    [BaseServices]
    private static IServiceCollection BaseServices => new ServiceCollection()
        .AddSingleton<IUserInfo, UserInfo>()
        .AddSingleton<RandomGuid>()
    ;
        
    [ServiceOverride("admin")]
    private static IServiceCollection AdminServices => new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid Guid => provider.GetService<RandomGuid>()!.Guid;

    public bool IsAdmin => provider.GetService<IUserInfo>()!.IsAdmin;
}

public class RegisteredDslWithNonStaticDefinitions(IServiceProvider provider)
{
    [BaseServices]
    private IServiceCollection BaseServices => new ServiceCollection()
        .AddSingleton<IUserInfo, UserInfo>()
        .AddSingleton<RandomGuid>()
    ;
        
    [ServiceOverride("admin")]
    private IServiceCollection AdminServices => new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid Guid => provider.GetService<RandomGuid>()!.Guid;

    public bool IsAdmin => provider.GetService<IUserInfo>()!.IsAdmin;
}