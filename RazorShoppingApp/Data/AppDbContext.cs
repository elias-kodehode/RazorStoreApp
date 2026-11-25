using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Data;
using RazorShoppingApp.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<TodoModel> Todos { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TodoModel>()
            .HasOne(x => x.User)
            .WithMany(x => x.Todos)
            .HasForeignKey(x => x.UserId);

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

        var now = DateTime.UtcNow;
        builder.Entity<TodoModel>()
            .HasData(new TodoModel
            {
                Id = 1,
                Title = "Add a Todo List!",
                Description = "A todo list to keep track of goals and such for this application",
                IsCompleted = false,
                CreatedDate = DateTime.Parse("25.11.2026"),
                UserId = "50909e80-48e2-45b1-a418-7a3a43ceb3e3",
            });

    }
}
