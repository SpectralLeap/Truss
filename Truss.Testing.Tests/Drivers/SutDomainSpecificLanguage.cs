using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl;
using Truss.Testing.Services;
using Truss.Testing.Tests.Fixtures;

namespace Truss.Testing.Tests.Drivers;

public class SutDomainSpecificLanguage(
    IUserInfo userInfo,
    IGuidProvider guidProvider,
    RegistrationStore registrationStore
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

    private readonly string _defaultEmail = $"{Guid.NewGuid()}@example.com";

    private readonly Guid _guid = Guid.NewGuid();
    
    public async Task RegisterUser(params string[] args)
    {
        var arguments = DslArgs
            .ForAction<RegisterUserDriver>()
            .From(
                args,
                DslParameter.Optional("email")
                    .SetDefault(_defaultEmail)
                    .SetPattern(@"(\w|\d)+@(\w|\d)+\.(\w){2,5}"),
                DslParameter.Optional("password")
            );

        await Act(arguments);
    }

    public void AssertRegistered(string? email = null)
    {
        email ??= _defaultEmail;
        
        Assert.True(registrationStore.Has(email), $"The expected email {email} was not registered");
    }
}