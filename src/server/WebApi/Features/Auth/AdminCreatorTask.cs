using Microsoft.AspNetCore.Identity;

namespace WebApi.Features.Auth;

public class AdminCreatorTask(IServiceProvider serviceProvider) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var admin = await userManager.FindByNameAsync("Admin");

        if (admin is null)
        {
            await userManager.CreateAsync(
                new IdentityUser
                {
                    UserName = "Admin"
                },
                "Password123!");
        }
    }
}
