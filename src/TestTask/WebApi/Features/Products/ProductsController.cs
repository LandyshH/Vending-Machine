using Application.Services.Brands;
using Application.Services.Products;
using Domain.Products;
using Domain.Products.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Products.Dtos;
using WebApi.Features.Products.Requests;

namespace WebApi.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;

        public ProductsController(IProductService productService, IBrandService brandService)
        {
            _productService = productService;
            _brandService = brandService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByFilter([FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] string? brandName)
        {
            var defaultMinPrice = 0;
            var defaultMaxPrice = await _productService.GetMaxPriceAsync();

            var filter = new ProductFilter
            {
                MinPrice = minPrice ?? defaultMinPrice,
                MaxPrice = maxPrice ?? defaultMaxPrice
            };

            if (!string.IsNullOrEmpty(brandName))
            {
                var brand = await _brandService.FindByNameAsync(brandName);

                (filter.MinPrice, filter.MaxPrice) = await _productService.GetPriceRangeByBrandAsync(brand);
                filter.Brand = brand;
            }

            var products = await _productService.GetProductsByFilterAsync(filter);
            return Ok(products);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Brand = product.Brand.Name,
                ImageUrl = product.Image.Url,
                Amount = product.Amount
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Brand = product.Brand.Name,
                ImageUrl = product.Image.Url,
                Amount = product.Amount
            };

            return Ok(productDto);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _brandService.GetBrandById(request.BrandId);

            var image = new Image(request.ImageUrl);
            var product = new Product(request.Name, request.Price, brand, image, request.Amount);

            await _productService.CreateProductAsync(product);

            return Ok(product);
        }

        [HttpGet("price-range-brand")]
        public async Task<IActionResult> GetProductsPriceRangeByBrand([FromQuery] int brandId)
        {
            var brand = await _brandService.GetBrandById(brandId);
            var (minPrice, maxPrice) = await _productService.GetPriceRangeByBrandAsync(brand);

            return Ok(new { minPrice, maxPrice });
        }

        [HttpGet("price-range")]
        public async Task<IActionResult> GetProductsPriceRange()
        {
            var (minPrice, maxPrice) = await _productService.GetPriceRangeAsync();

            return Ok(new { minPrice, maxPrice });
        }
    }
}
