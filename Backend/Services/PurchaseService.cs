using Microsoft.EntityFrameworkCore;
using shopping_list.DataBase;

namespace shopping_list.Services;

public interface IPurchaseService
{
    Task<List<Purchase>> GetAllPurchasesAsync();
    Task<Purchase?> GetPurchaseByIdAsync(string id);
    Task<Purchase> CreatePurchaseAsync(Purchase purchase);
    Task<Purchase?> UpdatePurchaseAsync(Purchase purchase);
    Task<Purchase?> DeletePurchaseAsync(string id);
    Task<List<Purchase>> GetAllPurchasesByUserIdAsync(string id);
}

public class PurchaseService : IPurchaseService
{
    private readonly ApplicationContext dbContext;

    public PurchaseService(ApplicationContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Purchase>> GetAllPurchasesAsync()
    {
        return await dbContext.Purchases.ToListAsync();
    }

    public async Task<Purchase?> GetPurchaseByIdAsync(string id)
    {
        return await dbContext.Purchases.FindAsync(id);
    }
    
    public async Task<List<Purchase>> GetAllPurchasesByUserIdAsync(string id)
    {
        return await dbContext.Purchases.Where(purchase => purchase.UserId == id).ToListAsync();
    }

    public async Task<Purchase> CreatePurchaseAsync(Purchase purchase)
    {
        purchase.Id = Guid.NewGuid().ToString();
        dbContext.Purchases.Add(purchase);
        await dbContext.SaveChangesAsync();
        return purchase;
    }

    public async Task<Purchase?> UpdatePurchaseAsync(Purchase purchase)
    {
        var existingUser = await dbContext.Purchases.FindAsync(purchase.Id);
        if (existingUser == null) return null;

        existingUser.Name = purchase.Name;
        existingUser.Count = purchase.Count;
        await dbContext.SaveChangesAsync();
        return existingUser;
    }

    public async Task<Purchase?> DeletePurchaseAsync(string id)
    {
        var user = await dbContext.Purchases.FindAsync(id);
        if (user == null) return null;

        dbContext.Purchases.Remove(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}