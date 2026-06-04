using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class StockController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
