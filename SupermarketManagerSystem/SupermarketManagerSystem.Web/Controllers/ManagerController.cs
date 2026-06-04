using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class ManagerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
