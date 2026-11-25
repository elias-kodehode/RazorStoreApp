using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class BasketService(
    AppDbContext db,
    ICurrentUserService currentUser)
{

    private readonly string _userId = currentUser.UserId ?? throw new UnauthorizedAccessException("User must be authenticated.");

    public async Task<Basket> GetOrCreateBasketAsync()
    {
        var basket = await db.Baskets.FirstOrDefaultAsync(x => x.UserId == _userId);

        if (basket is null)
        {
            basket = new Basket
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                UserId = _userId,
            };

            db.Baskets.Add(basket);
            await db.SaveChangesAsync();
        }

        return basket;
    }

    public async Task DeleteBasketAsync()
    {
        var basket = await db.Baskets.FirstOrDefaultAsync(x => x.UserId == _userId);

        if (basket is null)
            return;

        db.Baskets.Remove(basket);
        await db.SaveChangesAsync();
    }

    public async Task AddProductAsync(int productId, int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        var exists = await db.Products.AnyAsync(x => x.Id == productId);

        if (!exists)
            throw new KeyNotFoundException("Product not found.");

        var basket = await GetOrCreateBasketAsync();

        var basketItem = await db.BasketItems.FirstOrDefaultAsync(x => x.BasketId == basket.Id && x.ProductId == productId);

        if (basketItem is not null)
            basketItem.Quantity += quantity;
        else
        {
            basketItem = new BasketItem
            {
                BasketId = basket.Id,
                ProductId = productId,
                Quantity = quantity
            };

            db.BasketItems.Add(basketItem);
        }
        await db.SaveChangesAsync();
    }
}
