using Domain.Orders;
namespace Application.Services.Orders;

public interface IOrderService
{
    Task<Order> CreateNewOrderAsync();

    Task<ICollection<OrderItem>> GetAllOrderItemsAsync(int orderId);

    Task AddProductToOrderAsync(int orderId, int productId, int amount);

    Task DeleteProductFromOrderAsync(int orderId, int productId, int amount);

    Task ChangeOrderItemAmountAsync(int orderId, int productId, int amount);

    Task<Order> GetOrderByIdAsync(int orderId);

    Task<ICollection<Order>> GetAllOrdersAsync();
}
