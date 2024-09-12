namespace Domain.Coins;

public interface ICoinRepository
{
    Task<ICollection<Coin>> GetAllAsync();
    Task<Coin?> GetByDenominationAsync(CoinDenomination denomination);
    Task UpdateAsync(Coin coin);
}
