using Domain.Brands;

namespace Application.Services.Brands;

public interface IBrandService
{
    Task<ICollection<Brand>> GetAllBrandsAsync();
    Task<Brand> GetBrandById(int brandId);
    Task<Brand?> FindByNameAsync(string brandName);
}
