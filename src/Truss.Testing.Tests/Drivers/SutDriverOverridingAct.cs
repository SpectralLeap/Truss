using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Drivers;

public class SutDriverOverridingAct(RegistrationStore store)
    : Driver
{
    private const string LocalEmail = "local@local.com";
    private const string DrivenEmail = "driver@driver.com";

    [BaseServices] 
    private static IServiceCollection BaseServices = new ServiceCollection().AddSingleton<RegistrationStore>();
    
    public async Task RegisterUser(params string[] args)
    {
        var arguments = DslArgs
            .From(
                args,
                DslParameter.Optional("email")
                    .SetDefault(DrivenEmail)
                    .SetPattern(@"(\w|\d)+@(\w|\d)+\.(\w){2,5}"),
                DslParameter.Optional("password")
            );
    
        await Act(arguments);
    }

    protected override Task Act(DslArgs args)
    {
        var x = args["password"];

        store.AddData(LocalEmail);
        
        return Task.CompletedTask;
    }

    public void AssertBothActionsHappened()
    {
        Assert.True(store.Has(DrivenEmail));
        Assert.True(store.Has(LocalEmail));
    }

}