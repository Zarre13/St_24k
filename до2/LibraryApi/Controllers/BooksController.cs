using LibraryApi.Models;
using LibraryApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repository;

    public BooksController(IBookRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetAll(
        [FromQuery] string? author,
        [FromQuery] bool? available,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (page < 1)
            return BadRequest(new { error = "Параметр page должен быть больше 0." });

        if (limit is < 1 or > 100)
            return BadRequest(new { error = "Параметр limit должен находиться в диапазоне 1-100." });

        var books = await _repository.GetAllAsync(author, available, page, limit);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book)
    {
        if (await _repository.IsbnExistsAsync(book.Isbn))
            return Conflict(new { error = "A book with this ISBN already exists." });

        try
        {
            var created = await _repository.AddAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (DbUpdateException exception) when (IsUniqueViolation(exception))
        {
            return Conflict(new { error = "A book with this ISBN already exists." });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Book>> Update(int id, [FromBody] Book book)
    {
        if (id != book.Id)
            return BadRequest("ID в маршруте не совпадает с ID книги.");

        var existingBook = await _repository.GetByIdAsync(id);
        if (existingBook == null)
            return NotFound();

        if (await _repository.IsbnExistsAsync(book.Isbn, id))
            return Conflict(new { error = "A book with this ISBN already exists." });

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Year = book.Year;
        existingBook.Isbn = book.Isbn;
        existingBook.IsAvailable = book.IsAvailable;

        try
        {
            var updated = await _repository.UpdateAsync(existingBook);
            return Ok(updated);
        }
        catch (DbUpdateException exception) when (IsUniqueViolation(exception))
        {
            return Conflict(new { error = "A book with this ISBN already exists." });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingBook = await _repository.GetByIdAsync(id);
        if (existingBook == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    private static bool IsUniqueViolation(DbUpdateException exception)
        => exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation
        };
}