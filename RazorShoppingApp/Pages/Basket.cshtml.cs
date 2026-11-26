using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShoppingApp.Models;
using RazorShoppingApp.Services;

namespace RazorShoppingApp.Pages;

public class BasketModel(
    //AppDbContext db,
    BasketService basketService) : PageModel
{
    public Basket? Basket { get; set; } = null;

    public async Task<IActionResult> OnGetAsync()
    {
        this.Basket = await basketService.GetBasketAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await basketService.DeleteBasketAsync();
        this.Basket = null;
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync(int basketItemId, int quantity)
    {
        await basketService.UpdateQuantityAsync(basketItemId, quantity);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteBasketItemAsync(int id)
    {
        await basketService.DeleteBasketItemAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostPlaceOrder()
    {
        var basket = await basketService.GetBasketAsync();

        if (basket is null)
        {
            return RedirectToPage();
        }



        return RedirectToPage();
    }
}
