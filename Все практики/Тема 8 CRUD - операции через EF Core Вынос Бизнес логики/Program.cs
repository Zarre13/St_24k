using Library.Data;
using Library.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Регистрация DbContext (EF Core + PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookRepository, BookRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();   // ← ВОТ ЭТА СТРОКА БЫЛА ПРОПУЩЕНА
}

// Твой middleware (время ответа)
app.Use(async (context, next) =>
{
    var start = DateTime.UtcNow;
    await next();
    var response_time = (DateTime.UtcNow - start).TotalMilliseconds;
    Console.WriteLine($"Время ответа от сервера: {response_time}");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();