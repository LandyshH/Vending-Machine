namespace Domain.Coins;

public class Coin
{
    public int Id { get; set; }
    public CoinDenomination Denomination { get; private set; }
    public int Amount { get; private set; }

    public Coin()
    {
        
    }
    public Coin(CoinDenomination denomination, int amount)
    {
        Denomination = denomination;
        Amount = amount;
    }

    public bool TryChangeCoinAmount(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Amount = amount;
        return true;
    }
}
