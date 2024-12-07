using Microsoft.AspNetCore.Mvc;
using shopping_list.Services;

namespace shopping_list.Controllers;

[ApiController]
[Route("registration")]
public class RegistrationsController : ControllerBase
{
    private readonly IUserService userService;
    
    public RegistrationsController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet]
    [Route("")]
    public IActionResult Registration()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "registration.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }
        return Ok(user);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Registration([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Некорректные данные" });
        }

        var createdUser = await userService.RegistrationAsync(user);

        if (createdUser == null)
        {
            return BadRequest(new { message = "Пользователь с таким именем уже существует" });
        }
        
        return Ok();
    }
}