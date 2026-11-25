using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShoppingApp.Models;

namespace RazorShoppingApp.Pages.Dashboard.Baskets;

public class BasketsModel(AppDbContext db) : PageModel
{
    public Basket Basket { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var basket = await db.Baskets.FindAsync(id);

        if (basket is null)
            return RedirectToPage("/Dashboard");


        return Page();
    }
}
