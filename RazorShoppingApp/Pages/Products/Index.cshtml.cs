using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Pages.Products;

public class IndexModel(AppDbContext db) : PageModel
{
    public Product Product { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var product  = await db.Products.FindAsync(id);

        if(product is  null)
        {
            return NotFound();
        }
        this.Product = product;
        return Page();
    }
}
