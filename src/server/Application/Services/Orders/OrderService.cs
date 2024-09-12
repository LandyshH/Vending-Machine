using Domain.Coins;
using Domain.Orders;
using Domain.Products;

namespace Application.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICoinRepository _coinRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository,
                        ICoinRepository coinRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _coinRepository = coinRepository;
    }

    public async Task<Order> CreateNewOrderAsync()
    {
        var order = new Order();
        await _orderRepository.AddAsync(order);

        return order;
    }

    public async Task<ICollection<OrderItem>> GetAllOrderItemsAsync(int orderId)
    {
        var _currentOrder = await _orderRepository.GetByIdAsync(orderId);
        return _currentOrder.OrderItems.ToList();
    }

    public async Task AddProductToOrderAsync(int orderId, int productId, int amount)
    {
        var currentOrder = await _orderRepository.GetByIdAsync(orderId);
        var product = await _productRepository.GetByIdAsync(productId);

        if (!currentOrder.TryAddOrderItem(product, amount))
        {
            throw new ArgumentException("Can't add product to order");
        }

        currentOrder.CalculateTotalSum();

        await _orderRepository.SaveAsync(currentOrder);
    }

    public async Task DeleteProductFromOrderAsync(int orderId, int productId, int amount)
    {
        var currentOrder = await _orderRepository.GetByIdAsync(orderId);
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
        {
            throw new ArgumentException("Product is null");
        }

        if (!currentOrder.TryDeleteOrderItem(product))
        {
            throw new ArgumentException("Can't remove product from order");
        }

        if (!product.TryRemoveFromOrder(amount))
        {
            throw new ArgumentException("Can't remove product from order, amount less then 0");
        }

        currentOrder.CalculateTotalSum();

        await _orderRepository.SaveAsync(currentOrder);
    }

    public async Task ChangeOrderItemAmountAsync(int orderId, int productId, int amount)
    {
        var currentOrder = await _orderRepository.GetByIdAsync(orderId);
        var orderItem = currentOrder.OrderItems
            .FirstOrDefault(ot => ot.ProductDetails.ProductId == productId);

        if (orderItem is null)
        {
            throw new ArgumentException("Order item is null");
        }

        if (!orderItem.TryChangeOrderItemAmount(amount))
        {
            throw new ArgumentException("Can't change order item amount");
        }

        currentOrder.CalculateTotalSum();
        await _orderRepository.SaveAsync(currentOrder);
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<ICollection<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }
}
