using Domain.Brands;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public class Product
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public int Price { get; private set; }
    public Brand Brand { get; private set; }
    public Image Image { get; private set; }
    public int Amount { get; set; }

    public Product()
    {
        
    }

    public Product(string name, int price, Brand brand, Image image, int amount)
    {
        Name = name;
        Price = price;
        Brand = brand;
        Image = image;
        Amount = amount;
    }

    public bool TryAddToOrder(int amount)
    {
        if (amount > Amount)
        {
            return false;
        }

        Amount -= amount;

        return true;
    }

    public bool TryRemoveFromOrder(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Amount += amount;

        return true;
    }
}
