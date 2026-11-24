using Library.Data;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Library.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions
(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    Console.WriteLine("-> " + context.Request.Method + " " + path);
    // реализую вариант 2 - замер времени ответа сервера
    var response_time = Stopwatch.StartNew();

    await next(context);

    response_time.Stop();

    Console.WriteLine("<- " + context.Response.StatusCode);

    Console.WriteLine($"Время ответа от сервера: {response_time}");

});

app.UseAuthorization();

app.MapControllers();

app.Run();
