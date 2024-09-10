using Domain.Brands;
using Domain.Coins;
using Domain.Orders;
using Domain.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.PostgreSQL;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{   
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Coin> Coins { get; set; }

    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(e => e.Image, image =>
            {
                image.Property(i => i.Url).HasColumnName("ImageUrl");
            });
        });

        modelBuilder.Entity<Coin>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.TotalSum).IsRequired();

            entity.HasMany(e => e.OrderItems)
                  .WithOne()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.OwnsOne(e => e.ProductDetails, navigation =>
            {
                navigation.Property(p => p.ProductId).HasColumnName("ProductId");
                navigation.Property(p => p.Brand).HasColumnName("Brand");
                navigation.Property(p => p.ProductName).HasColumnName("ProductName");
                navigation.Property(p => p.Price).HasColumnName("Price");
                navigation.Property(p => p.ImageUrl).HasColumnName("ImageUrl");
            });
        });
    }
}
    
