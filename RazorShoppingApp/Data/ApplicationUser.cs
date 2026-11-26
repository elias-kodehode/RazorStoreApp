using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using RazorShoppingApp.Models;
namespace RazorShoppingApp.Data;

public class ApplicationUser : IdentityUser
{
    public Basket? Basket { get; set; }
    /// <summary>
    /// Credit in NOK
    /// </summary>
    public decimal Credit { get; set; } = 250000;
    public ICollection<Order> Orders { get; set; } = [];
}


public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}