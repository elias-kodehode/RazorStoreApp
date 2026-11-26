using RazorShoppingApp.Data;

namespace RazorShoppingApp.Models;

public class Basket
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<BasketItem> Items { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public decimal CalculateTotal()
    {
        return Items.Sum(item => item.Product.Price * item.Quantity);
    }
}
