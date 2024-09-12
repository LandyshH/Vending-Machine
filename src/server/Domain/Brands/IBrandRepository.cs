namespace Domain.Brands;

public interface IBrandRepository
{
    Task<ICollection<Brand>> GetAllAsync();
    Task<Brand> GetByIdAsync(int id);
    Task<Brand?> FindByNameAsync(string name);
    Task AddAsync(Brand brand);
}
