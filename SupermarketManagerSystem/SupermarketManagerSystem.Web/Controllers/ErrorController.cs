using Microsoft.AspNetCore.Mvc;

namespace SupermarketManagerSystem.Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleError(int statusCode)
    {
        return statusCode switch
        {
            400 => View("BadRequest"),
            401 => View("Unauthorized"),
            403 => View("Unauthorized"),
            404 => View("NotFound"),
            _ => View("InternalServerError")
        };
    }

    [Route("Error/500")]
    public IActionResult ServerError() => View("InternalServerError");
}
