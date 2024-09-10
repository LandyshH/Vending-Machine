using Microsoft.EntityFrameworkCore;
using Domain.Coins;
using Infrastructure.Data.PostgreSQL;

namespace Infrastructure.Data.Repositories;

public class CoinRepository : ICoinRepository
{
    private readonly ApplicationDbContext _context;

    public CoinRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Coin>> GetAllAsync()
    {
        return await _context.Coins.ToListAsync();
    }

    public async Task<Coin?> GetByDenominationAsync(CoinDenomination denomination)
    {
        return await _context.Coins
                             .SingleOrDefaultAsync(c => c.Denomination == denomination);
    }

    public async Task UpdateAsync(Coin coin)
    {
        _context.Coins.Update(coin);
        await _context.SaveChangesAsync();
    }
}

