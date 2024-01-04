using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;
using Truss.Dsl;
using Truss.Dsl.Arguments;
using Truss.Dsl.Parameters;
using Truss.Tests.Dsl.Fixtures;

namespace Truss.Tests.Core;


public class SutDsl(
    IUserInfo userInfo,
    IGuidProvider guidProvider,
    RegistrationStore registrationStore
    )
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
    
    [DslMethod]
    public virtual DslArgs RegisterUser(params string[] args)
    {
        return DslArgs
            .ForAction<RegisterUser>()
            .From(
            args,
            DslParameter.Optional("email")
                .SetDefault(_defaultEmail)
                .SetPattern(@"(\w|\d)+@(\w|\d)+\.(\w){2,5}"),
            DslParameter.Optional("password")
        );
    }

    /// <summary>
    /// Methods must be virtual to interact with the proxy
    ///
    /// If not virtual then volatile state will differ
    /// </summary>
    /// <param name="email"></param>
    public virtual void AssertRegistered(string? email = null)
    {
        email ??= _defaultEmail;
        
        Assert.True(registrationStore.Has(email), $"The expected email {email} was not registered");
    }
}

public sealed class RegistrationStore
{
    private readonly List<string> _dataBase = new();
    
    public void AddData(string data)
    {
        _dataBase.Add(data);    
    }

    public bool Has(string data)
    {
        return _dataBase.Contains(data);
    }
}

public class RegisterUser;


public sealed class RegisterUserDriver(RegistrationStore registrationStore) : Driver<RegisterUser>
{
    public override void Drive(DslArgs args)
    {
        var email = args["email"]!;
        
        registrationStore.AddData(email);
    }
}

public sealed class AssertionTests
{
    private readonly DomainDslFactory _factoryFixture = new();
     
    [Fact]
    public void TheDriverIsCalledApplyingDefaultParameters()
    {
        var system = _factoryFixture.GetDsl<SutDsl>();
        
        system.RegisterUser();
        system.RegisterUser();
        
        system.AssertRegistered();
    }

    [Fact]
    public void CanUseOverrides()
    {
        var adminSystem = _factoryFixture.GetDsl<SutDsl>(tags: "admin");
        var userSystem = _factoryFixture.GetDsl<SutDsl>();

        Assert.True(adminSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
    }
    
    [Fact]
    public void CanUseMultipleOverrides()
    {
        var overriddenSystem = _factoryFixture.GetDsl<SutDsl>(tags: ["admin", "empty guid"]);
        var userSystem = _factoryFixture.GetDsl<SutDsl>();

        Assert.True(overriddenSystem.UserInfo.IsAdmin);
        Assert.False(userSystem.UserInfo.IsAdmin);
        
        Assert.NotEqual(Guid.Empty, userSystem.guidProvider.Guid);
        Assert.Equal(Guid.Empty, overriddenSystem.guidProvider.Guid);
    }
}