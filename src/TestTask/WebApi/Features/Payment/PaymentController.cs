using Application.Services.Payment;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Payment.Requests;

namespace WebApi.Features.Payment;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("coins")]
    public async Task<IActionResult> GetAllCoinsAsync()
    {
        var coins = await _paymentService.GetAllCoins();
        return Ok(coins);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> PayOrderAsync([FromBody] CoinsRequest insertedCoins)
    {
        var result = await _paymentService.PayOrderAsync(insertedCoins.OrderId, insertedCoins.Coins);

        return Ok(new
        {
            result.Message,
            result.IsChangeGiven,
            result.ChangeCoins,
            ChangeAmount = result.ChangeCoins?.Sum(x => (int)x.Key * x.Value) ?? 0
        });
    }

    [HttpPost("change-coins")]
    public async Task<IActionResult> ChangeCoinsAmountAsync([FromBody] CoinsRequest coinUpdates)
    {
        foreach (var update in coinUpdates.Coins)
        {
            await _paymentService.ChangeCoinsAmountAsync(update.Key, update.Value);
        }

        return Ok();

    }
}
