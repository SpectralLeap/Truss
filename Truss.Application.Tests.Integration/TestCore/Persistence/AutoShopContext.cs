using Microsoft.EntityFrameworkCore;

namespace Truss.Application.Tests.Integration;

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

        modelBuilder.Entity<AutoShop>().Navigation(shop => shop.Garages).AutoInclude();
        modelBuilder.Entity<Garage>().Navigation(shop => shop.Cars).AutoInclude();
    }
}