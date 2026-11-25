using System.ComponentModel.DataAnnotations;

namespace RazorShoppingApp.Models;

public class ProductInput
{
    [Required, MinLength(4), MaxLength(30)]
    public string Name { get; set; } = "";

    [Required, MinLength(1), MaxLength(500)]
    public string Description { get; set; } = "";

    [Required, Range(0, int.MaxValue)]
    public decimal Price { get; set; }


    public string? ImageUrl { get; set; } = null;

    public ProductInput()
    {
        
    }

    public ProductInput(Product product)
    {
        this.Name = product.Name;
        this.Description = product.Description;
        this.Price = product.Price;
        this.ImageUrl = product.ImageUrl;
    }
}
