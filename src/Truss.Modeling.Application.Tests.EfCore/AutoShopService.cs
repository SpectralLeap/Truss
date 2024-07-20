using Truss.Modeling.Application.Tests.EfCore.Persistence;
using Truss.Modeling.Application.Tests.TestCore.Domain;
using Truss.Monads.Results;

namespace Truss.Modeling.Application.Tests.EfCore;

public sealed class AutoShopService
{
    private readonly AutoShopContext _context;

    public AutoShopService(AutoShopContext context)
    {
        _context = context;
    }
    
    public Result<Nil> AddAutoShop(AutoShop autoShop)
    {
        _context.Add(autoShop);
        
        _context.SaveChanges();
        
        return Result.Success();
    }

    public Result<AutoShop> GetAutoShop(AutoShopId autoShopId)
    {
        var shop = _context.AutoShops
            .FirstOrDefault(autoShop => autoShop.Id == autoShopId);
        
        return shop is null ? Result.Fail() : Result.Success(shop);
    }
}