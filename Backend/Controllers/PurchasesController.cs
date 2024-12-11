using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping_list.Contracts;
using shopping_list.DataAccess;
using shopping_list.Models;

namespace shopping_list.Controllers;

    [ApiController]
    [Route("api")]
    public class PurchasesController(ShopingListDbContext shopingListDbContext) : ControllerBase
    {
        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetAllPurchasesByListId(CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            
            var purchases = await shopingListDbContext.Purchases.Where(p => p.UserId == userId.Value).ToListAsync(ct);
            return Ok(purchases);
        }
        
        [HttpGet]
        [Route("getshare")]
        public async Task<IActionResult> GetShare([FromQuery] string listId, CancellationToken ct)
        {
            var purchases = await shopingListDbContext.Purchases.Where(p => p.ListId == listId).ToListAsync(ct);
            return Ok(purchases);
        }

        [HttpGet]
        [Route("purchase")]
        public async Task<IActionResult> GetPurchaseById([FromQuery] GetPurchaseByIdRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            var purchase = await shopingListDbContext.Purchases.Where(p => p.Id == request.Id).FirstOrDefaultAsync(ct);
            if (purchase == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }
            
            return Ok(purchase);
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var purchase = new Purchase(request.Name, request.Count, request.Type,
                request.ListId, userId.Value);

            await shopingListDbContext.Purchases.AddAsync(purchase, ct);
            await shopingListDbContext.SaveChangesAsync(ct);
            
            return Ok(purchase);
        }

        [HttpPut]
        [Route("purchase")]
        public async Task<IActionResult> UpdatePurchase([FromBody] PutPurchaseRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }
            
            var purchase = await shopingListDbContext.Purchases.Where(p => p.Id == request.Id).FirstOrDefaultAsync(ct);
            if (purchase == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }
            
            purchase.Count = request.Count;
            purchase.Name = request.Name;
            purchase.Type = request.Type;

            await shopingListDbContext.SaveChangesAsync(ct);
            
            return Ok(purchase);
        }

        [HttpDelete]
        [Route("purchase")]
        public async Task<IActionResult> DeletePurchase([FromBody] DeletePurchaseRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }
            
            var purchase = await shopingListDbContext.Purchases.Where(p => p.Id == request.Id).FirstOrDefaultAsync(ct);
            if (purchase == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }

            shopingListDbContext.Purchases.Remove(purchase);
            await shopingListDbContext.SaveChangesAsync(ct);
            
            return Ok(purchase);
        }
    }