using Domain.Brands;
using Domain.Products;
using Infrastructure.Data.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .ToListAsync();
        }

        public async Task<ICollection<Product>> GetByFiltersAsync(ProductFilter filter)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .AsQueryable();

            if (filter.Brand != null)
            {
                query = query.Where(p => p.Brand == filter.Brand);
            }

            query = query.Where(p => p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice);

            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .FirstAsync(p => p.Id == id);
        }

        public async Task<(int MinPrice, int MaxPrice)> GetPriceRangeByBrandAsync(Brand? brand)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .AsQueryable();

            query = query.Where(p => p.Brand == brand);

            var minPrice = await query.MinAsync(p => p.Price);
            var maxPrice = await query.MaxAsync(p => p.Price);

            return (minPrice, maxPrice);
        }

        public async Task<(int MinPrice, int MaxPrice)> GetPriceRangeAsync()
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .AsQueryable();

            var minPrice = await query.MinAsync(p => p.Price);
            var maxPrice = await query.MaxAsync(p => p.Price);

            return (minPrice, maxPrice);
        }

        public async Task<int> GetMaxPriceAsync()
        {
            return await _context.Products.MaxAsync(p => p.Price);
        }
    }
}
