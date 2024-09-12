using Domain.Coins;

namespace WebApi.Features.Payment.Requests;

public class CoinsRequest
{
    public int OrderId { get; set; }
    public required Dictionary<CoinDenomination, int> Coins { get; set; }
}
