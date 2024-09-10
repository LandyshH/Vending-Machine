namespace WebApi.Features.Products.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int BrandId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int Amount { get; set; }
}
