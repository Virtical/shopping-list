using Microsoft.EntityFrameworkCore;
using shopping_list;
using static System.Text.RegularExpressions.Regex;
 
var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql("Host=178.253.43.74;Port=5432;Database=purchases_db;Username=gen_user;Password=I=YAv5XsNiBZ(B"));

var app = builder.Build();
 
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;
    
    const string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    
    if (path == "/api/users" && request.Method=="GET")
    {
        await GetAllPurchases(response, dbContext); 
    }
    else if (IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        var id = path.Value?.Split("/")[3];
        await GetPurchase(id, response, dbContext);
    }
    else if (path == "/api/users" && request.Method == "POST")
    {
        await CreatePurchase(response, request, dbContext);
    }
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdatePurchase(response, request, dbContext);
    }
    else if (IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        string? id = path.Value?.Split("/")[3];
        await DeletePurchase(id, response, dbContext);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        response.Headers.ContentLanguage = "ru-RU";
        await response.SendFileAsync("html/index.html");
    }
});
 
app.Run();

async Task GetAllPurchases(HttpResponse response, ApplicationContext dbContext)
{
    var purchases = await dbContext.Purchases.ToListAsync();
    await response.WriteAsJsonAsync(purchases);
}

async Task GetPurchase(string? id, HttpResponse response, ApplicationContext dbContext)
{
    var purchase = await dbContext.Purchases.FindAsync(id);
    if (purchase != null)
    {
        await response.WriteAsJsonAsync(purchase);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Покупка не найдена" });
    }
}
 
async Task DeletePurchase(string? id, HttpResponse response, ApplicationContext dbContext)
{
    var purchase = await dbContext.Purchases.FindAsync(id);
    if (purchase != null)
    {
        dbContext.Purchases.Remove(purchase);
        await dbContext.SaveChangesAsync();
        await response.WriteAsJsonAsync(purchase);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Покупка не найдена" });
    }
}
 
async Task CreatePurchase(HttpResponse response, HttpRequest request, ApplicationContext dbContext)
{
    try
    {
        var purchase = await request.ReadFromJsonAsync<Purchase>();
        if (purchase != null)
        {
            purchase.Id = Guid.NewGuid().ToString();
            dbContext.Purchases.Add(purchase);
            await dbContext.SaveChangesAsync();
            await response.WriteAsJsonAsync(purchase);
        }
        else
        {
            throw new Exception("Некорректные данные");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}
 
async Task UpdatePurchase(HttpResponse response, HttpRequest request, ApplicationContext dbContext)
{
    try
    {
        var purchaseData = await request.ReadFromJsonAsync<Purchase>();
        if (purchaseData != null)
        {
            var purchase = await dbContext.Purchases.FindAsync(purchaseData.Id);
            if (purchase != null)
            {
                purchase.Name = purchaseData.Name;
                purchase.Count = purchaseData.Count;
                await dbContext.SaveChangesAsync();
                await response.WriteAsJsonAsync(purchase);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Покупка не найдена" });
            }
        }
        else
        {
            throw new Exception("Некорректные данные");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}