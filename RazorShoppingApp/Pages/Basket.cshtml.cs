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
}
