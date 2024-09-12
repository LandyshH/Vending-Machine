using Domain.Coins;

namespace Application.Services.Payment;

public interface IPaymentService
{
    Task<ICollection<Coin>> GetAllCoins();
    Task<PaymentResult> PayOrderAsync(int orderId, Dictionary<CoinDenomination, int> insertedCoins);
    Task ChangeCoinsAmountAsync(CoinDenomination denomination, int amount);
}
