using Domain.Brands;
using Domain.Products;

namespace Application.Services.Products;

public class ProductService : IProductService
{
    IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<ICollection<Product>> GetProductsByFilterAsync(ProductFilter filter)
    {
        return await _productRepository.GetByFiltersAsync(filter);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<(int MinPrice, int MaxPrice)> GetPriceRangeByBrandAsync(Brand brand)
    {
        return await _productRepository.GetPriceRangeByBrandAsync(brand);
    }

    public async Task<(int MinPrice, int MaxPrice)> GetPriceRangeAsync()
    {
        return await _productRepository.GetPriceRangeAsync();
    }

    public async Task CreateProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
    }

    public async Task<int> GetMaxPriceAsync()
    {
        return await _productRepository.GetMaxPriceAsync();
    }
}
