using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Tests.EfCore.TestCore;
using Truss.Modeling.Application.Tests.EfCore.TestCore.Domain;
using Truss.Testing;
using Truss.Testing.Drivers;
using Truss.Testing.Dsl;
using Truss.Testing.Services;

namespace Truss.Modeling.Application.Tests.EfCore;

public class AutoShopFixture 
    : Fixture
{
    private readonly AutoShopService _autoShopService;

    [BaseServices] 
    public static IServiceCollection Services => new ServiceCollection()
        .AddTestCore()
    ;

    public AutoShopFixture(AutoShopService autoShopService)
    {
        _autoShopService = autoShopService;
    }
    
    public async Task AddAndGetShopUsingDslAndDriver(params string[] args)
    {
        var arguments = DslArgs
            .ForAction<AddAndGetShopAction>()
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

    public class AddAndGetShopAction
    {
        
    }
    
    public class AddAndGetShopDriver : Driver<AddAndGetShopAction>
    {
        private readonly AutoShopService _autoShopService;

        public AddAndGetShopDriver(AutoShopService autoShopService)
        {
            _autoShopService = autoShopService;
        }
        
        public override async Task Drive(DslArgs args)
        {
            var id = new AutoShopId(Guid.NewGuid());
            var shop = new AutoShop(id, args["name"]!);

            _autoShopService.AddAutoShop(shop);

            var shopAgain = _autoShopService.GetAutoShop(id).SuccessValue;
            
            Assert.Equal(shop.Name, shopAgain.Name);
        }
    }
}