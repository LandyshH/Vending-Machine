using Infrastructure;
using Infrastructure.Data.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = ConfigureServices();
using var scope = services.CreateScope();
return;

static async Task MigrateDatabaseAsync(ApplicationDbContext applicationDbContext)
{
    await applicationDbContext.Database.MigrateAsync();
}


static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services
        .AddDefaultIdentity<IdentityUser>()
        .AddEntityFrameworkStores<ApplicationDbContext>();


    return services.BuildServiceProvider();
}