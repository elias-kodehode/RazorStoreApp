using RazorShoppingApp.Models;
using System.Security.Authentication;

namespace RazorShoppingApp.Services;

public class OrderService(
    AppDbContext db,
    BasketService basketService,
    ICurrentUser currentUser)
{
    public async Task CheckoutAsync()
    {
        if (!currentUser.IsAuthenticated) throw new AuthenticationException();

        var basket = await basketService.GetBasketAsync();

        if (basket is null) throw new();

        if (basket.Items.Count == 0) throw new();

        var now = DateTime.UtcNow;
        if (basket.ExpiresAt < now)
        {
            await basketService.DeleteBasketAsync();
            return;
        }

        var user = await currentUser.GetUserAsync() ?? throw new();


        if (user.Credit < basket.CalculateTotal())
            return;

        var order = new Order
        {
            Items = basket.Items.Select(item => new OrderItem
            {

            });
        }
}
