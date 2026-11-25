using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Pages;

[Authorize]
public class DashboardModel(AppDbContext db) : PageModel
{
    public List<Product> Products { get; set; } = [];
    public List<Basket> Baskets { get; set; } = [];


    [BindProperty]
    public ProductInput Input { get; set; } = default!;

    public Task OnGetAsync() => LoadProductsAsync();

    private async Task LoadProductsAsync()
    {
        this.Products = await db.Products.ToListAsync();
        this.Baskets = await db.Baskets.Include(basket => basket.Items).ToListAsync();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadProductsAsync();
            return Page();
        }


        var product = new Product
        {
            Name = Input.Name,
            Description = Input.Description,
            Price = Input.Price
        };

        db.Products.Add(product);


        await db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var product = await db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        db.Remove(product);
        await db.SaveChangesAsync();
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostDeleteBasketAsync(int id)
    {
        var basket = await db.Baskets.FindAsync(id);

        if (basket is null)
            return NotFound();

        db.Remove(basket);
        await db.SaveChangesAsync();
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostDeleteBasketsAsync()
    {
        var deleted = await db.Baskets.ExecuteDeleteAsync();
        return RedirectToPage();
    }
}
