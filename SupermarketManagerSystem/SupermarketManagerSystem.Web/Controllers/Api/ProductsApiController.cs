using Microsoft.AspNetCore.Mvc;
using SupermarketManagerSystem.Services.Contracts;

namespace SupermarketManagerSystem.Web.Controllers.Api;

[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsApiController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId)
    {
        var products = await _productService.GetAllAsync(search, categoryId);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound(new { message = "Product not found." });
        return Ok(product);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var products = await _productService.GetLowStockProductsAsync();
        return Ok(products);
    }
}
