using Infrastructure;
using Infrastructure.Data.PostgreSQL;
using Infrastructure.Data.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrator;

var services = ConfigureServices();
using var scope = services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var s3Service = scope.ServiceProvider.GetRequiredService<IS3Service>();
var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

await MigrateDatabaseAsync(dbContext);
await UploadFilesToS3Async(s3Service);
await AddDefaultDataAsync(dataSeeder);

return;

static async Task MigrateDatabaseAsync(ApplicationDbContext migratorDbContext)
{
    await migratorDbContext.Database.MigrateAsync();
}

static async Task UploadFilesToS3Async(IS3Service s3Service)
{
    await s3Service.EnsureFileBucketExistsAsync();

    var directory = Directory.GetCurrentDirectory();

    var files = Directory.GetFiles(directory)
        .Select(p => p.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last().Split("/", StringSplitOptions.RemoveEmptyEntries).Last())
        .Where(f => f.StartsWith("image"));
    foreach (var file in files)
    {
        var fileName = Path.GetFileName(file);
        await using var fileStream = File.OpenRead(file);
        var uri = await s3Service.UploadFileAsync(fileStream, fileName);
    }
}

static async Task AddDefaultDataAsync(IDataSeeder dataSeeder)
{
    await dataSeeder.SeedAsync();
}

static IServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

    services.AddSingleton(configuration);
    services.AddSingleton<IConfiguration>(configuration);

    var connectionString = configuration.GetConnectionString("Postgres");

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

    services.AddS3Configuration(configuration);

    services.AddScoped<IDataSeeder, DataSeeder>();


    return services.BuildServiceProvider();
}