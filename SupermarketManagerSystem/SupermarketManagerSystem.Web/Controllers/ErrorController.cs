using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
