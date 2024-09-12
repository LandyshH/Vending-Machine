using Domain.Brands;
namespace Domain.Products;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);

    Task<ICollection<Product>> GetAllAsync();

    Task<ICollection<Product>> GetByFiltersAsync(ProductFilter filter);

    Task AddAsync(Product product);

    Task<(int MinPrice, int MaxPrice)> GetPriceRangeByBrandAsync(Brand brand);

    Task<(int MinPrice, int MaxPrice)> GetPriceRangeAsync();

    Task<int> GetMaxPriceAsync();
}
