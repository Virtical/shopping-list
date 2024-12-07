using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shopping_list.Controllers;

[ApiController]
[Authorize]
[Route("/list")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "home.html");
        
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Файл list.html не найден.");
        }
        
        return PhysicalFile(filePath, "text/html");
    }
}