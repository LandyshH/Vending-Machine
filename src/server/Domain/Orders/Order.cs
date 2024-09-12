using Domain.Products;

namespace Domain.Orders;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
    public int TotalSum { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Order()
    {
        
    }

    public bool TryAddOrderItem(Product product, int amount)
    {
        if (_orderItems.Any(item => item.ProductDetails.ProductId == product.Id))
        {
            return false;  
        }

        if (!product.TryAddToOrder(amount))
        {
            return false;
        }

        var orderItem = new OrderItem(product.Id, product.Brand.Name, product.Name, product.Price, amount, product.Image.Url);
        _orderItems.Add(orderItem);

        CalculateTotalSum();

        return true;
    }

    public bool TryDeleteOrderItem(Product product)
    {
        var itemToRemove = _orderItems.FirstOrDefault(item => item.ProductDetails.ProductId == product.Id);
        if (itemToRemove is not null)
        {
            if (!product.TryRemoveFromOrder(itemToRemove.Amount))
            {
                return false;
            }

            _orderItems.Remove(itemToRemove);

            CalculateTotalSum();
            return true;
        }

        return false;
    }

    public void CalculateTotalSum()
    {
        TotalSum = _orderItems.Sum(item => item.ProductDetails.Price * item.Amount);
    }
}
