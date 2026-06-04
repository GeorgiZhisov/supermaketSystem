using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers.Api;

[ApiController]
[Route("api/stock")]
public class StockApiController : ControllerBase
{
    [HttpGet("low-stock")]
    public IActionResult GetLowStockProducts()
    {
        return Ok(new List<object>());
    }
}
