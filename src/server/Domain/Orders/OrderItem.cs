using Domain.Orders.ValueObjects;

namespace Domain.Orders;

public class OrderItem
{
    public int Id { get; set; }

    public OrderItemValueObject ProductDetails { get; private set; }

    public int Amount { get; private set; }
    
    public OrderItem(int productId, string brand, string productName, int price, int amount, string imageUrl)
    {
        ProductDetails = new OrderItemValueObject(productId, brand, productName, price, imageUrl);
        Amount = amount;
    }

    private OrderItem()
    {

    }

    public bool TryChangeOrderItemAmount(int amount)
    {
        if (amount < 0) return false;

        Amount = amount;
        return true;
    }
}
