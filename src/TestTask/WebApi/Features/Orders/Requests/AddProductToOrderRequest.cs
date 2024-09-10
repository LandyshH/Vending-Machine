namespace WebApi.Features.Orders.Requests;

public class AddProductToOrderRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
}
