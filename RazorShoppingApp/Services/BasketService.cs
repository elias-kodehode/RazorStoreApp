using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class BasketService(
    AppDbContext db,
    ICurrentUser currentUser)
{
    private static readonly TimeSpan BasketLifetime = TimeSpan.FromDays(5);
    private readonly string _userId = currentUser.UserId ?? throw new UnauthorizedAccessException("User must be authenticated.");


    private async Task<Basket> GetOrCreateBasketAsync()
    {
        var basket = await db
            .Baskets
            .Include(basket => basket.Items)
            .FirstOrDefaultAsync(x => x.UserId == _userId);

        var now = DateTime.UtcNow;

        //return early if a basket exists and it has not expired
        if (basket is not null && basket.ExpiresAt > now)
            return basket;

        if (basket is null)
        {
            basket = new Basket
            {
                UserId = _userId,
            };
            db.Baskets.Add(basket);
        }

        basket.ExpiresAt = now.AddMinutes(5);
        basket.Items.Clear();
        basket.CreatedAt = now;
        await db.SaveChangesAsync();
        return basket;
    }


    public Task<Basket?> GetBasketAsync()
    {
        return db
            .Baskets
            .Include(basket => basket.Items)
            .ThenInclude((BasketItem item) => item.Product)
            .FirstOrDefaultAsync(basket => basket.UserId == _userId && basket.ExpiresAt > DateTime.UtcNow);
    }


    //public async Task DeleteBasketAsync()
    //{
    //    var basket = await db.Baskets.FirstOrDefaultAsync(x => x.UserId == _userId);

    //    if (basket is null)
    //        return;

    //    db.Baskets.Remove(basket);
    //    await db.SaveChangesAsync();
    //}

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

        RefreshExpiration(basket);
        await db.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int basketItemId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than or equal to 1");


        var basketItem = await GetBasketItemAsync(basketItemId) ?? throw new KeyNotFoundException("Basket item not found.");


        basketItem.Quantity = quantity;


        RefreshExpiration(basketItem.Basket);

        await db.SaveChangesAsync();
    }

    private static void RefreshExpiration(Basket basket)
    {
        basket.ExpiresAt = DateTime.UtcNow.Add(BasketLifetime);
    }

    private Task<BasketItem?> GetBasketItemAsync(int basketItemId)
    {
        return db
            .BasketItems
            .Include(item => item.Basket)
            .FirstOrDefaultAsync(item =>
                item.Id == basketItemId &&
                item.Basket.UserId == _userId &&
                item.Basket.ExpiresAt > DateTime.UtcNow);
    }

    public async Task DeleteBasketItemAsync(int basketItemId)
    {
        var basketItem = await GetBasketItemAsync(basketItemId)
                ?? throw new KeyNotFoundException("Basket item not found.");

        db.BasketItems.Remove(basketItem);
        await db.SaveChangesAsync();
    }

}
