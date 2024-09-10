using Domain.Brands;

namespace Domain.Products;

public class ProductFilter
{
    public int MinPrice { get; set; }
    public int MaxPrice { get; set; }
    public Brand? Brand { get; set; }
}
