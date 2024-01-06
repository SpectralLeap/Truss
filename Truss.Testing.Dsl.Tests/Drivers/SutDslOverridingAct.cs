using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Dsl.Services;
using Truss.Testing.Dsl.Tests.Core;

namespace Truss.Testing.Dsl.Tests.Drivers;

public class SutDslOverridingAct(RegistrationStore store) : Dsl
{
    private const string LocalEmail = "local@local.com";
    private const string DrivenEmail = "driver@driver.com";

    [BaseServices] private static IServiceCollection BaseServices = new ServiceCollection().AddSingleton<RegistrationStore>();
    
    public async Task RegisterUser(params string[] args)
    {
        var arguments = DslArgs
            .ForAction<RegisterUser>()
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
        store.AddData(LocalEmail);
        
        return Task.CompletedTask;
    }

    public void AssertBothActionsHappened()
    {
        store.Has(DrivenEmail);
        store.Has(LocalEmail);
    }

}