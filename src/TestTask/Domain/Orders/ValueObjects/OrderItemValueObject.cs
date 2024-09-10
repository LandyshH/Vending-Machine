namespace Domain.Orders.ValueObjects;

public class OrderItemValueObject
{
    public string Brand { get; private set; }
    public string ProductName { get; private set; }
    public int Price { get; private set; }
    public int ProductId { get; private set; }
    public string ImageUrl { get; set; }

    public OrderItemValueObject(int productId, string brand, string productName, int price, string imageUrl)
    {
        ProductId = productId;
        Brand = brand;
        ProductName = productName;
        Price = price;
        ImageUrl = imageUrl;
    }
}
