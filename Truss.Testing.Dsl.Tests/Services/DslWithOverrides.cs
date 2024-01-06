using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;
using Truss.Testing.Dsl.Tests.Drivers;
using Truss.Testing.Dsl.Tests.Fixtures;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslWithOverrides(
    IUserInfo userInfo,
    IGuidProvider guidProvider
)  : Dsl
{
    public IUserInfo UserInfo { get; } = userInfo;
    
    public IGuidProvider guidProvider { get; } = guidProvider;

    [BaseServices] 
    public static IServiceCollection Services = new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RegistrationStore>()
            .AddSingleton<IGuidProvider, RandomGuid>()
        ;
    
    [ServiceOverride(tag: "admin")] 
    public static IServiceCollection AdminServices = new ServiceCollection()
            .AddSingleton<IUserInfo, AdminInfo>()
        ;
    
    [ServiceOverride(tag: "empty guid")] 
    public static IServiceCollection EmptyGuidServices = new ServiceCollection()
            .AddSingleton<IGuidProvider, EmptyGuid>()
        ;
}