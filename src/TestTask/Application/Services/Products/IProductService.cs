using Domain.Brands;
using Domain.Products;
namespace Application.Services.Products;

public interface IProductService
{
    Task<ICollection<Product>> GetAllProductsAsync();
    Task<ICollection<Product>> GetProductsByFilterAsync(ProductFilter filter);
    Task<Product?> GetByIdAsync(int id);
    Task<(int MinPrice, int MaxPrice)> GetPriceRangeByBrandAsync(Brand brand);
    Task<(int MinPrice, int MaxPrice)> GetPriceRangeAsync();
    Task CreateProductAsync(Product product);
    Task<int> GetMaxPriceAsync();
}
