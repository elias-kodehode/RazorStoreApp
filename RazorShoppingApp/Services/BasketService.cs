using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Data;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Services;

public class BasketService(
    AppDbContext db,
    ICurrentUserService currentUser)
{

    private readonly string _userId = currentUser.UserId ?? throw new Exception();

    public async Task<Basket> GetOrCreateBasket()
    {
        var basket = await db.Baskets.FirstOrDefaultAsync(x => x.UserId == _userId);

        if(basket is null)
        {
            basket = new Basket {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                UserId = _userId,
            };

            db.Baskets.Add(basket);
            await db.SaveChangesAsync();
        }

        return basket;
    }

    public async Task DeleteBasket()
    {
        var basket = await db.Baskets.FirstOrDefaultAsync(x => x.UserId == _userId);

        if (basket is null)
            return;

        db.Baskets.Remove(basket);
        await db.SaveChangesAsync();
    }

    public async Task AddProduct(int productId, int quantity = 1)
    {
        var basketItem = new BasketItem { };
    }
}
