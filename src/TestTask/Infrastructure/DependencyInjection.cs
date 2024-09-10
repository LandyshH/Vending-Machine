using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Domain.Products;
using Infrastructure.Data.Repositories;
using Domain.Orders;
using Domain.Coins;
using Domain.Brands;
using Amazon.S3;
using Infrastructure.Data.S3;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICoinRepository, CoinRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();

        return services;
    }

    public static IServiceCollection AddS3Configuration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<S3Options>().Bind(configuration.GetSection("AWS"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<S3Options>>().Value;

            Console.WriteLine($"ServiceURL: {options.ServiceURL}");
            Console.WriteLine($"AccessKey: {options.AccessKey}");
            Console.WriteLine($"SecretKey: {options.SecretKey}");

            var config = new AmazonS3Config
            {
                ServiceURL = options.ServiceURL,
                ForcePathStyle = true,
                UseHttp = true
            };


            return new AmazonS3Client(options.AccessKey, options.SecretKey, config);
        });

        services.AddSingleton<IS3Service, S3Service>();

        return services;
    }
}
