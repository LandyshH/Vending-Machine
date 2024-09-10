using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.Services.Orders;
using Application.Services.Payment;
using Application.Services.Products;
using Application.Services.Brands;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBrandService, BrandService>();

        return services;
    }
}
