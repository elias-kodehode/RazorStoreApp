using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Data;
using RazorShoppingApp.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Basket>()
            .HasOne(basket => basket.User)
            .WithOne(user => user.Basket)
            .HasForeignKey<Basket>(basket => basket.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<BasketItem>()
            .HasOne(item => item.Basket)
            .WithMany(basket => basket.Items)
            .HasForeignKey(item => item.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BasketItem>()
            .HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<BasketItem>()
            .HasIndex(item => new { item.BasketId, item.ProductId })
            .IsUnique();

        builder.Entity<OrderItem>()
            .HasIndex(item => new { item.OrderId, item.ProductId })
            .IsUnique();

        builder.Entity<OrderItem>()
            .Property(item => item.UnitPrice)
            .HasPrecision(18, 2);

        builder.Entity<Product>()
            .Property(product => product.Price)
            .HasPrecision(18, 2);

        builder.Entity<ApplicationUser>()
            .Property(user => user.Credit)
            .HasPrecision(18, 2);

        builder.Entity<OrderItem>()
            .Property(item => item.ProductName)
            .HasMaxLength(200);
    }
}
