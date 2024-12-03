using shopping_list;
using static System.Text.RegularExpressions.Regex;

var purchases = new List<Purchase> 
{ 
    new() { Id = Guid.NewGuid().ToString(), Name = "Яйца", Count = 1 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Шоколадка", Count = 2 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Печенье", Count = 3 }
};
 
var builder = WebApplication.CreateBuilder();
var app = builder.Build();
 
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;
    
    const string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    if (path == "/api/users" && request.Method=="GET")
    {
        await GetAllPurchases(response); 
    }
    else if (IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        var id = path.Value?.Split("/")[3];
        await GetPurchase(id, response);
    }
    else if (path == "/api/users" && request.Method == "POST")
    {
        await CreatePurchase(response, request);
    }
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdatePurchase(response, request);
    }
    else if (IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        string? id = path.Value?.Split("/")[3];
        await DeletePurchase(id, response);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        response.Headers.ContentLanguage = "ru-RU";
        await response.SendFileAsync("html/index.html");
    }
});
 
app.Run();

async Task GetAllPurchases(HttpResponse response)
{
    await response.WriteAsJsonAsync(purchases);
}

async Task GetPurchase(string? id, HttpResponse response)
{
    var purchase = purchases.FirstOrDefault((u) => u.Id == id);
    if (purchase != null)
        await response.WriteAsJsonAsync(purchase);
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Покупка не найдена" });
    }
}
 
async Task DeletePurchase(string? id, HttpResponse response)
{
    var purchase = purchases.FirstOrDefault((u) => u.Id == id);
    if (purchase != null)
    {
        purchases.Remove(purchase);
        await response.WriteAsJsonAsync(purchase);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Покупка не найдена" });
    }
}
 
async Task CreatePurchase(HttpResponse response, HttpRequest request)
{
    try
    {
        var purchase = await request.ReadFromJsonAsync<Purchase>();
        if (purchase != null)
        {
            purchase.Id = Guid.NewGuid().ToString();
            purchases.Add(purchase);
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
 
async Task UpdatePurchase(HttpResponse response, HttpRequest request)
{
    try
    {
        var purchaseData = await request.ReadFromJsonAsync<Purchase>();
        if (purchaseData != null)
        {
            var purchase = purchases.FirstOrDefault(u => u.Id == purchaseData.Id);
            if (purchase != null)
            {
                purchase.Count = purchaseData.Count;
                purchase.Name = purchaseData.Name;
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