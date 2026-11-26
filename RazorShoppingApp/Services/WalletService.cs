using System.Security.Authentication;

namespace RazorShoppingApp.Services;

public class WalletService(
    ICurrentUser currentUser,
    AppDbContext db)
{
    public async Task<decimal> GetCredit()
    {
        var user = await currentUser.GetUserAsync();

        if (user is null)
            throw new AuthenticationException();

        return user.Credit;
    }


}
