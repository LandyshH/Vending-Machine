using Domain.Coins;
namespace Application.Services.Payment;

public class PaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsChangeGiven { get; set; }
    public Dictionary<CoinDenomination, int> ChangeCoins { get; set; }
}
