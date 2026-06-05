using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
}
