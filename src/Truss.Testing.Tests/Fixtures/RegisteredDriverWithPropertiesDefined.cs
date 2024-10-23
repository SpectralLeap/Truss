using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Fixtures;

public class RegisteredDriverWithPropertiesDefined(IServiceProvider provider) : Driver
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