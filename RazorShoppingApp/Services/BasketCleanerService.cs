using Microsoft.EntityFrameworkCore;

namespace RazorShoppingApp.Services;

public class BasketCleanerService(
    IServiceScopeFactory serviceScope, 
    ILogger<BasketCleanerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScope.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var now = DateTime.UtcNow;


                var amtOfBaskets = await db.Baskets.CountAsync(stoppingToken);
                logger.LogInformation("Found {Count} baskets.", amtOfBaskets);

                var deleted = await db.Baskets.Where(x => x.ExpiresAt < now).ExecuteDeleteAsync(stoppingToken);
                logger.LogInformation("Basket cleanup completed. Deleted {Count} baskets.", deleted);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to clean up expired baskets.");
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
