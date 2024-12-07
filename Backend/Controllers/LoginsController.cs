using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using shopping_list.Services;

namespace shopping_list.Controllers;

[ApiController]
[Route("login")]
public class LoginsController : ControllerBase
{
    private readonly IUserService userService;
    
    public LoginsController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet]
    [Route("")]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "login.html");
    
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("HTML файл не найден.");
        }
    
        return PhysicalFile(filePath, "text/html");
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Email и/или пароль не установлены" });
        }

        var registeredUser = await userService.GetUserByLoginAndPasswordAsync(user.Login, user.Password);

        if (registeredUser is null)
        {
            return Unauthorized(new { message = "Неверные логин или пароль" });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, registeredUser.Login),
            new Claim(ClaimTypes.NameIdentifier, registeredUser.Id)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            });

        return Ok(new { redirectUrl = $"list"});
    }


    [HttpGet]
    [Route("logout")]
    public async Task Logout()
    {
        var prop = new AuthenticationProperties()
        {
            RedirectUri = "/"
        };
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, prop);
    }
}