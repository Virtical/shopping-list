var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    response.Headers.ContentLanguage = "ru-RU";
    response.ContentType = "text/html; charset=utf-8";
    await response.WriteAsync("<h1>Список покупок</h1>");
});

app.Run();