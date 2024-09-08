using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;
using Truss.Testing.Tests.Drivers;
using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests.Services;

public class DomainSpecificLanguageWithOverrides(
    IUserInfo userInfo,
    IGuidProvider guidProvider
)  : DomainSpecificLanguage
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