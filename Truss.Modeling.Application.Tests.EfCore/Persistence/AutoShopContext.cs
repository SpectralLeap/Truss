using Microsoft.EntityFrameworkCore;
using Truss.Modeling.Application.Tests.TestCore.Domain;

namespace Truss.Modeling.Application.Tests.EfCore.Persistence;

public sealed class AutoShopContext : DbContext
{
    public DbSet<AutoShop> AutoShops { get; set; }
    
    public DbSet<Garage> Garages { get; set; }

    public AutoShopContext(DbContextOptions<AutoShopContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AutoShopConfiguration().Configure(modelBuilder.Entity<AutoShop>());
        new GarageConfiguration().Configure(modelBuilder.Entity<Garage>());
        new CarConfiguration().Configure(modelBuilder.Entity<Car>());

        modelBuilder.Entity<AutoShop>().Navigation(shop => shop.Garages).AutoInclude();
        modelBuilder.Entity<Garage>().Navigation(shop => shop.Cars).AutoInclude();
    }
}