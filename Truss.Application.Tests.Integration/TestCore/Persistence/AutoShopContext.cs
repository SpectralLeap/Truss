using Microsoft.EntityFrameworkCore;
using Truss.Application.Tests.Integration.TestCore.Domain;

namespace Truss.Application.Tests.Integration.TestCore.Persistence;

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