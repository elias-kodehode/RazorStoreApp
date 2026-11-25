using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Pages.Dashboard.Products;

[Authorize]
public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public ProductInput Input { get; set; } = default!;

    [BindProperty(SupportsGet =true)]
    private Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {

        var product  = await db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        this.Product = product;
        this.Input = new(this.Product);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        var product = await db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        product.Name = Input.Name;
        product.Description = Input.Description;
        product.Price = Input.Price;

        await db.SaveChangesAsync();
        this.TempData["Success"] = "Product updated successfully";
        return RedirectToPage("/Products");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        return RedirectToPage("/Products");
    }
}
