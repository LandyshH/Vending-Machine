using Application.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Orders.Requests;

namespace WebApi.Features.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create-new-order")]
    public async Task<IActionResult> CreateNewOrderAsync()
    {
        var order = await _orderService.CreateNewOrderAsync();
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    [HttpGet("order-items")]
    public async Task<IActionResult> GetCurrentOrderItemsAsync([FromQuery] int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        return Ok(order.OrderItems);
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToOrderAsync([FromBody] AddProductToOrderRequest request)
    {
        var order = await _orderService.GetOrderByIdAsync(request.OrderId);
        await _orderService.AddProductToOrderAsync(request.OrderId, request.ProductId, request.Amount);

        return Ok(order.OrderItems);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteProductFromOrderAsync([FromBody] DeleteProductFromOrderRequest request)
    {
        await _orderService.DeleteProductFromOrderAsync(request.OrderId, request.ProductId, request.Amount);
        return Ok();

    }

    [HttpPost("change-amount")]
    public async Task<IActionResult> ChangeOrderItemAmountAsync([FromBody] ChangeOrderItemAmountRequest request)
    {
        await _orderService.ChangeOrderItemAmountAsync(request.OrderId, request.ProductId, request.Amount);
        return Ok();

    }

    [HttpGet("sum")]
    public async Task<IActionResult> GetTotalSumAsync([FromQuery] int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        return Ok(new { order.TotalSum });
    }
}
