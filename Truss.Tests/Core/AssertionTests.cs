using Microsoft.Extensions.DependencyInjection;
using Truss.Drivers;
using Truss.Dsl;
using Truss.Dsl.Arguments;
using Truss.Dsl.Parameters;
using Truss.Tests.Dsl.Fixtures;

namespace Truss.Tests.Core;


public sealed class SutDsl
{
    [BaseServices] 
    public static IServiceCollection Services = new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
        ;

    [OverrideServices(tag: "admin")] 
    public static IServiceCollection AdminServices = new ServiceCollection()
            .AddSingleton<IUserInfo, AdminInfo>()
        ;
    
    [DslMethod]
    public DslArgs RegisterUser(params string[] args)
    {
        return DslArgs.From(
            args,
            DslParameter.Optional("email")
                .SetPattern(@"(\w|\d)+@(\w|\d)+\.(\w){2,5}"),
            DslParameter.Optional("password")
        );
    }
}

public class RegisterUser
{
}

public sealed class RegisterUserDriver(IIntegrationBus integrationBus) : Driver<RegisterUser>(integrationBus)
{
    public override void Drive(DslArgs args)
    {
        var email = args["email"]!;
        
        Report<SumIs>(new SumIs());
    }
}

public sealed class AssertionTests
{
    private readonly DomainDslFactory _factoryFixture = new();
     
    [Fact]
    public void AssertionsApply()
    {
        var calculator = _factoryFixture.GetDsl<SutDsl>();
        
    }   
}