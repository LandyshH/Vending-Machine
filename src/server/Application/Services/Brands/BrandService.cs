using Domain.Brands;

namespace Application.Services.Brands;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<ICollection<Brand>> GetAllBrandsAsync()
    {
        return await _brandRepository.GetAllAsync();
    }

    public async Task<Brand> GetBrandById(int brandId)
    {
       return await _brandRepository.GetByIdAsync(brandId);
    }

    public async Task<Brand?> FindByNameAsync(string brandName)
    {
        return await _brandRepository.FindByNameAsync(brandName);
    }
}
