using Truss.Drivers;
using Truss.Dsl;
using Truss.Dsl.Arguments;
using Truss.Dsl.Parameters;
using Truss.Tests.Dsl.Fixtures;

namespace Truss.Tests.Core;


public sealed class SutDsl(IIntegrationBus integrationBus) : DomainDsl(integrationBus)
{
    public void RegisterUser(params string[] args)
    {
        var arguments = DslArgs.From(
            args,
            DslParameter.Optional("email")
                .SetPattern(@"(\w|\d)+@(\w|\d)+\.(\w){2,5}"),
            DslParameter.Optional("password")
        );

        Act<RegisterUser>(arguments);
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
    private readonly SutFactoryFixture _factoryFixture = new();
     
    [Fact]
    public void AssertionsApply()
    {
        var calculator = _factoryFixture.GetDsl<SutDsl>();
        
        calculator.Assert<SumIs>(s => s.Expected = 10);
    }   
}