using Domain.Orders;
using Infrastructure.Data.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstAsync(o => o.Id == id);
    }

    public async Task SaveAsync(Order order)
    {
        await _context.SaveChangesAsync(); 
    }
}
