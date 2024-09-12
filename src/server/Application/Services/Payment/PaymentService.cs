using Application.Services.Orders;
using Domain.Coins;

namespace Application.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IOrderService _orderService;

        public PaymentService(ICoinRepository coinRepository, IOrderService orderService)
        {
            _coinRepository = coinRepository;
            _orderService = orderService;
        }

        public async Task<ICollection<Coin>> GetAllCoins()
        {
            return await _coinRepository.GetAllAsync();
        }

        public async Task<PaymentResult> PayOrderAsync(int orderId, Dictionary<CoinDenomination, int> insertedCoins)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            int totalInsertedAmount = insertedCoins.Sum(x => (int)x.Key * x.Value);

            if (totalInsertedAmount < order.TotalSum)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = "Insufficient funds",
                    IsChangeGiven = false
                };
            }

            int change = totalInsertedAmount - order.TotalSum;
            var changeCoins = await CalculateChangeAsync(change);

            if (changeCoins == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = "Sorry, we cannot provide the correct change",
                    IsChangeGiven = false
                };
            }

            await UpdateCoinsAsync(insertedCoins, changeCoins);

            return new PaymentResult
            {
                Success = true,
                Message = "Thank you for your purchase!",
                IsChangeGiven = true,
                ChangeCoins = changeCoins
            };
        }

        public async Task ChangeCoinsAmountAsync(CoinDenomination denomination, int amount)
        {
            var coin = await _coinRepository.GetByDenominationAsync(denomination);

            if (coin == null)
            {
                throw new ArgumentException("Coin not found");
            }

            if (!coin.TryChangeCoinAmount(amount))
            {
                throw new ArgumentException("Invalid amount for coin");
            }

            await _coinRepository.UpdateAsync(coin);
        }

        private async Task<Dictionary<CoinDenomination, int>?> CalculateChangeAsync(int changeAmount)
        {
            var allCoins = await _coinRepository.GetAllAsync();
            var availableCoins = allCoins.OrderByDescending(c => (int)c.Denomination);
            var changeCoins = new Dictionary<CoinDenomination, int>();

            foreach (var coin in availableCoins)
            {
                int coinValue = (int)coin.Denomination;
                int neededCoins = changeAmount / coinValue;

                if (neededCoins > 0 && coin.Amount >= neededCoins)
                {
                    changeCoins[coin.Denomination] = neededCoins;
                    changeAmount -= neededCoins * coinValue;
                }
            }

            return changeAmount == 0 ? changeCoins : null;
        }

        private async Task UpdateCoinsAsync(Dictionary<CoinDenomination, int> insertedCoins,
                                            Dictionary<CoinDenomination, int> changeCoins)
        {
            foreach (var insertedCoin in insertedCoins)
            {
                var coin = await _coinRepository.GetByDenominationAsync(insertedCoin.Key);
                if (coin != null)
                {
                    coin.TryChangeCoinAmount(coin.Amount + insertedCoin.Value);
                    await _coinRepository.UpdateAsync(coin);
                }
            }

            foreach (var changeCoin in changeCoins)
            {
                var coin = await _coinRepository.GetByDenominationAsync(changeCoin.Key);
                if (coin != null)
                {
                    coin.TryChangeCoinAmount(coin.Amount - changeCoin.Value);
                    await _coinRepository.UpdateAsync(coin);
                }
            }
        }
    }
}
