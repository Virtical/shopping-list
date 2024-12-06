using Microsoft.AspNetCore.Mvc;
using shopping_list.Services;

namespace shopping_list.Controllers;

    [ApiController]
    [Route("api")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var users = await purchaseService.GetAllPurchasesAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("purchases/{id}")]
        public async Task<IActionResult> GetPurchaseById(string id)
        {
            var user = await purchaseService.GetPurchaseByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("purchases")]
        public async Task<IActionResult> CreatePurchase([FromBody] Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            var createdUser = await purchaseService.CreatePurchaseAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        [Route("purchases")]
        public async Task<IActionResult> UpdatePurchase([FromBody] Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            var updatedUser = await purchaseService.UpdatePurchaseAsync(purchase);
            if (updatedUser == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }
            return Ok(updatedUser);
        }

        [HttpDelete]
        [Route("purchases/{id}")]
        public async Task<IActionResult> DeletePurchase(string id)
        {
            var deletedUser = await purchaseService.DeletePurchaseAsync(id);
            if (deletedUser == null)
            {
                return NotFound(new { message = "Покупка не найдена" });
            }
            return Ok(deletedUser);
        }
    }