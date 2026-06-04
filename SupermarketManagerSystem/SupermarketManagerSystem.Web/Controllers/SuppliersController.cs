using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class SuppliersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
