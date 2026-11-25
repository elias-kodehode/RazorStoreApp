using Microsoft.AspNetCore.Identity;
using RazorShoppingApp.Data;

namespace RazorShoppingApp.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager) : ICurrentUserService
{
    private readonly HttpContext Context = httpContextAccessor.HttpContext ?? throw new Exception();

    public string? UserId => userManager.GetUserId(Context.User);
    public string? Username => userManager.GetUserName(Context.User);
    
}


public interface ICurrentUserService
{
    public string? UserId { get; }
    public string? Username { get; }
}