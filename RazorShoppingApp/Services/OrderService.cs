using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class OrderService(
    AppDbContext db,
    BasketService basketService,
    ICurrentUser currentUser)
{
    public async Task<Order> CheckoutAsync()
    {
        if (!currentUser.IsAuthenticated) throw new UnauthorizedAccessException();

        var basket = await basketService.GetBasketAsync() ?? throw new KeyNotFoundException("Basket not found");

        if (basket.Items.Count == 0) throw new InvalidOperationException("basket is empty");

        //var now = DateTime.UtcNow;

        //if (basket.ExpiresAt <= now)
        //{
        //    await basketService.DeleteBasketAsync();
        //    throw new InvalidOperationException("Basket has expired");
        //}

        var user = await currentUser.GetUserAsync() ?? throw new KeyNotFoundException("User not found");
        var totalPrice = basket.CalculateTotal();

        if (user.Credit < totalPrice)
            throw new InvalidOperationException("Not enough credit");


        List<OrderItem> items = [.. basket.Items.Select(item => new OrderItem
        {
            ProductName = item.Product.Name,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.Product.Price,
        })];

        var order = new Order
        {
            Items = items,
            UserId = user.Id,
        };

        db.Orders.Add(order);
        db.Baskets.Remove(basket);
        user.Credit -= totalPrice;


        await db.SaveChangesAsync();
        return order;
    }
}
