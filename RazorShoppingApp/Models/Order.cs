using RazorShoppingApp.Data;

namespace RazorShoppingApp.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal CalculateTotal()
    {
        return Items.Sum(item => item.UnitPrice * item.Quantity);
    }
}
