using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Data;
using RazorShoppingApp.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Basket>()
            .HasOne(x => x.User)
            .WithOne(x => x.Basket)
            .HasForeignKey<Basket>(x => x.UserId);


        builder.Entity<Basket>()
            .HasIndex(x => x.UserId)
            .IsUnique();

        builder.Entity<BasketItem>()
            .HasIndex(x => new { x.BasketId, x.ProductId })
            .IsUnique();

    }
}
