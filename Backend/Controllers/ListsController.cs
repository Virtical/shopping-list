using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping_list.Contracts;
using shopping_list.DataAccess;
using shopping_list.Models;

namespace shopping_list.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ListsController(ShopingListDbContext shopingListDbContext) : ControllerBase
{
    [HttpGet]
    [Route("lists")]
    public async Task<IActionResult> GetAllLists(CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        var lists = await shopingListDbContext.Lists.Where(l => l.UserId == userId.Value).ToListAsync(ct);
        return Ok(lists);
    }
    
    [HttpPost]
    [Route("list")]
    public async Task<IActionResult> CreateList([FromBody] CreateListRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Некорректные данные" });
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        var list = new List(request.Name, userId.Value);

        await shopingListDbContext.Lists.AddAsync(list, ct);
        await shopingListDbContext.SaveChangesAsync(ct);
            
        return Ok(list);
    }
    
    [HttpDelete]
    [Route("list")]
    public async Task<IActionResult> DeleteList([FromBody] DeleteListRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Некорректные данные" });
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        var list = await shopingListDbContext.Lists.Where(l => l.Id == request.Id).FirstOrDefaultAsync(ct);
        if (list is null)
        {
            return NotFound(new { message = "Лист не найден" });
        }
        shopingListDbContext.Lists.Remove(list);
        await shopingListDbContext.SaveChangesAsync(ct);
            
        return Ok(list);
    }
}