using Microsoft.AspNetCore.Identity;
using RazorShoppingApp.Data;

namespace RazorShoppingApp;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        var adminEmail = "admin@admin.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if(adminUser is null)
        {
            adminUser = new ApplicationUser { 
                UserName = "Admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (!result.Succeeded)
                throw new Exception(string.Join(", ",result.Errors.Select(x => x.Description)));

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
