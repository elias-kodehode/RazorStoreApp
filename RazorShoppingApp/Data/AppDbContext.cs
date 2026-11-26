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
            .HasForeignKey<Basket>(basket => basket.UserId);

        builder.Entity<Order>()
            .HasOne(order => order.User)
            .WithMany(items => items.Orders);
    }
}
