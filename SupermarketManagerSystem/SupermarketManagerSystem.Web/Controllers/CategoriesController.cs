using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class CategoriesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
