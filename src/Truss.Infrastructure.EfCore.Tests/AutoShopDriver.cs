using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Tests.TestCore.Domain;
using Truss.Testing;
using Truss.Testing.Dsl;
using Truss.Testing.Services;

namespace Truss.Infrastructure.EfCore.Tests;

public class AutoShopDriver 
    : Driver
{
    private readonly AutoShopService _autoShopService;

    [BaseServices] 
    public static IServiceCollection Services => new ServiceCollection()
        .AddTestCore()
    ;

    public AutoShopDriver(
        AutoShopService autoShopService
    )
    {
        _autoShopService = autoShopService;
    }
    
    public async Task AddAndGetShopUsingDslAndDriver(params string[] args)
    {
        var arguments = DslArgs
            .From(
                args,
                DslParameter.Optional("name")
                    .SetDefault("Midas")
                    .SetPattern(@"\w+"),
                DslParameter.Optional("cars")
                    .SetDefault("toyota camry, ford pinto")
                    .SetPattern(@"\w+")
                    .AsList()
            );
    
        await Act(arguments);
    }

    public void AddAndGetShopOnDsl()
    {
        var id = new AutoShopId(Guid.NewGuid());
        var shop = new AutoShop(id, "Monkey People Auto");
 
        _autoShopService.AddAutoShop(shop);
 
        var shopAgain = _autoShopService.GetAutoShop(id).SuccessValue;
             
        Assert.Equal(shop.Name, shopAgain.Name);       
    }
}