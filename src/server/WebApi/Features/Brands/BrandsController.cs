using Application.Services.Brands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Features.Brands;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var products = await _brandService.GetAllBrandsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var brand = await _brandService.GetBrandById(id);
        return Ok(brand);
    }
}
