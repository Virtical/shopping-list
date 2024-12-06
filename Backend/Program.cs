using Microsoft.EntityFrameworkCore;
using shopping_list;
using shopping_list.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql("Host=178.253.43.74;Port=5432;Database=purchases_db;Username=gen_user;Password=I=YAv5XsNiBZ(B"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IPurchaseService, PurchaseService>();

var app = builder.Build();

app.UseCors();

app.UseSwagger(); 
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.Run();