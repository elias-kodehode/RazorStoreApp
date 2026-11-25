using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Models;
using RazorShoppingApp.Services;

namespace RazorShoppingApp.Pages;

public class IndexModel(
    AppDbContext db,
    BasketService basketService) : PageModel
{
    public List<Product> Products { get; set; } = [];

    public Task OnGetAsync() => LoadProductsAsync();

    private async Task LoadProductsAsync()
    {
        this.Products = await db.Products.ToListAsync();
    }


    public async Task<IActionResult> OnGetProductDetailsAsync(int id)
    {
        var product = await db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        return Partial("_ProductDetails", product);
    }

    public async Task<IActionResult> OnPostAddToBasketAsync(int productId)
    {
        await basketService.AddProductAsync(productId);
        return RedirectToPage();
    }

}

