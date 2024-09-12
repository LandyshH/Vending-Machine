namespace WebApi.Features.Products.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Brand { get; set; }
    public string ImageUrl { get; set; } 
    public int Amount { get; set; }
    public bool Available => Amount > 0;
}
