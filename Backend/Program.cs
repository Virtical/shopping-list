using Microsoft.EntityFrameworkCore;
using shopping_list;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql("Host=178.253.43.74;Port=5432;Database=purchases_db;Username=gen_user;Password=I=YAv5XsNiBZ(B"));

var app = builder.Build();

// Главная страница
app.Map("/", async (context) =>
{
    var response = context.Response;
    response.ContentType = "text/html; charset=utf-8";
    response.Headers.ContentLanguage = "ru-RU";
    await response.SendFileAsync("html/index.html");
});

// Получить всех пользователей
app.Map($"/api/users", async (context) =>
{
    var response = context.Response;
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await GetAllPurchases(response, dbContext);
});

// Получить пользователя по ID
app.Map($"/api/user/{{id}}", async (HttpContext context, string id) =>
{
    var response = context.Response;
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await GetPurchase(id, response, dbContext);
});

// Добавить нового пользователя
app.MapPost("/api/createuser", async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await CreatePurchase(response, request, dbContext);
});

// Обновить данные пользователя
app.MapPut("/api/edituser", async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await UpdatePurchase(response, request, dbContext);
});

// Удалить пользователя
app.MapDelete($"/api/deleteuser/{{id}}", async (HttpContext context, string id) =>
{
    var response = context.Response;
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await DeletePurchase(id, response, dbContext);
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