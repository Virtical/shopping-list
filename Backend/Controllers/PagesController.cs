using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shopping_list.Controllers;

[ApiController]
public class PagesController : ControllerBase
{
    [HttpGet]
    [Route("/")]
    public IActionResult Main()
    {
        return Redirect(User.Identity.IsAuthenticated ? "/list" : "/login");
    }
    
    [HttpGet]
    [Route("/share")]
    public IActionResult Share()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/share", "index.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }
    
    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/login", "index.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }
    
    [HttpGet]
    [Route("registration")]
    public IActionResult Registration()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/registration", "index.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }
    
    [HttpGet]
    [Authorize]
    [Route("list")]
    public IActionResult List()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/list", "index.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }
}