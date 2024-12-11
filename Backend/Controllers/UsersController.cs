using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping_list.Contracts;
using shopping_list.DataAccess;
using shopping_list.Models;

namespace shopping_list.Controllers;

[ApiController]
public class UsersController(ShopingListDbContext shopingListDbContext) : ControllerBase
{
        [HttpGet]
        [Authorize]
        [Route("username")]
        public IActionResult GetUserName()
        {
            var userNameClaim = User.FindFirst(ClaimTypes.Name);
            var userName = userNameClaim?.Value;
            return Ok(new { name = userName });
        }
        
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> UserRegistration([FromBody] CreateUserRegistrationRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            var user = await shopingListDbContext.Users.FirstOrDefaultAsync(u => u.Login == request.Login, ct);
            if (user is not null)
            {
                return BadRequest(new { message = "Такой логин уже существует" });
            }

            user = new Users(request.Login, request.Password);

            await shopingListDbContext.Users.AddAsync(user, ct);
            await shopingListDbContext.SaveChangesAsync(ct);

            return Ok(new { message = "Регистрация успешна" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] CreateUserRegistrationRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные логин или пароль" });
            }

            var user = shopingListDbContext.Users.FirstOrDefault(u => u.Login == request.Login && u.Password == request.Password);
            if (user is null)
            {
                return Unauthorized(new { message = "Неверные логин или пароль" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
                });

            return Redirect("/list");
        }


        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Успешный выход");
        }
}