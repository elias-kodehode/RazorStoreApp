using Microsoft.AspNetCore.Identity;
using RazorShoppingApp.Data;
using System.Security.Claims;

namespace RazorShoppingApp.Services;

public class CurrentUser(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager) : ICurrentUser
{
    private ClaimsPrincipal? Principal
    {
        get
        {
            var principal = httpContextAccessor.HttpContext?.User;

            if (principal?.Identity?.IsAuthenticated != true)
                return null;

            return principal;
        }
    }

    public string? UserId => Principal is { } principal ? userManager.GetUserId(principal) : null;
    public string? Username => Principal is { } principal ? userManager.GetUserName(principal) : null;
    public bool IsAuthenticated => Principal is not null;

    public Task<ApplicationUser?> GetUserAsync() =>
        Principal is { } principal ? userManager.GetUserAsync(principal) : Task.FromResult<ApplicationUser?>(null);

}


public interface ICurrentUser
{
    public string? UserId { get; }
    public string? Username { get; }

    public bool IsAuthenticated { get; }
    Task<ApplicationUser?> GetUserAsync();
}