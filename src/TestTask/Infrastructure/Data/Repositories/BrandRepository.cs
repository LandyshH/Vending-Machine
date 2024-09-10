using Domain.Brands;
using Infrastructure.Data.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Brand brand)
    {
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Brand>> GetAllAsync() => 
        await _context.Brands.ToListAsync();
    

    public async Task<Brand> GetByIdAsync(int id) => 
        await _context.Brands.FirstAsync(b => b.Id == id);

    public async Task<Brand?> FindByNameAsync(string brandName)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.Name == brandName);
    }
}
