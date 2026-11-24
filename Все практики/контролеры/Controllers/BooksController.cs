using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static readonly List<Book> Books =
    [
        new() { Id = 1, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", PublicationYear = 1967, Price = 24.50m },
        new() { Id = 2, Title = "Война и мир", Author = "Лев Толстой", PublicationYear = 1869, Price = 32.00m },
        new() { Id = 3, Title = "Преступление и наказание", Author = "Фёдор Достоевский", PublicationYear = 1866, Price = 39.90m }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(Books);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = Books.FirstOrDefault(book => book.Id == id);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpGet("by-title/{title}")]
    public ActionResult<Book> GetByTitle(string title)
    {
        var book = Books.FirstOrDefault(book =>
            book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        return book is null ? NotFound() : Ok(book);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Book>> Search(string title)
    {
        var results = Books
            .Where(book => book.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return Ok(results);
    }
}