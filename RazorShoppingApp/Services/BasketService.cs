using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class BasketService(
    AppDbContext db,
    ICurrentUser currentUser)
{

    private readonly string _userId = currentUser.UserId ?? throw new UnauthorizedAccessException("User must be authenticated.");

    private async Task<Basket> GetOrCreateBasketAsync()
    {
        var basket = await db
            .Baskets
            .FirstOrDefaultAsync(x => x.UserId == _userId);

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


    public async Task<Basket?> GetBasketAsync()
    {
        var basket = await db
            .Baskets
            .Include(basket => basket.Items)
            .ThenInclude((BasketItem item) => item.Product)
            .FirstOrDefaultAsync(x => x.UserId == _userId);


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

    public async Task UpdateQuantityAsync(int basketItemId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than or equal to 1");


        var basketItem = await GetBasketItemAsync(basketItemId) ?? throw new KeyNotFoundException("Basket item not found.");


        basketItem.Quantity = quantity;
        await db.SaveChangesAsync();
    }

    private Task<BasketItem?> GetBasketItemAsync(int basketItemId)
    {
        return db
            .BasketItems
            .FirstOrDefaultAsync(item => item.Id == basketItemId && item.Basket.UserId == _userId);
    }

    public async Task DeleteBasketItemAsync(int basketItemId)
    {
        var basketItem = await GetBasketItemAsync(basketItemId)
                ?? throw new KeyNotFoundException("Basket item not found.");

        db.BasketItems.Remove(basketItem);
        await db.SaveChangesAsync();
    }


    public Task<decimal> CalculatePrice()
    {
        //TODO
        throw new Exception();
    }
}
