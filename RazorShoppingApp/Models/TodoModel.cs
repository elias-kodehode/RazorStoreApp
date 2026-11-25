using RazorShoppingApp.Data;

namespace RazorShoppingApp.Models;

public class TodoModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;



    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedDate { get; set; } = null;

}
