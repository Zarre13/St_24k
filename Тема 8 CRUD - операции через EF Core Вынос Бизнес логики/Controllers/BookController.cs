using Library.Models;
using Library.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _repository;

    public BookController(IBookRepository repository)
    {
        _repository = repository;
    }

    // GET: api/book
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(_repository.GetAll());
    }

    // GET: api/book/{id}
    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();
        return book;
    }

    // GET: api/book/search?author=...&minPrice=...
    [HttpGet("search")]
    public ActionResult<IEnumerable<Book>> Search(
        [FromQuery] string? author,
        [FromQuery] decimal? minPrice)
    {
        return Ok(_repository.Search(author, minPrice));
    }

    // GET: api/book/filter
    [HttpGet("filter")]
    public ActionResult<IEnumerable<Book>> Filter([FromQuery] BookFilter filter)
    {
        return Ok(_repository.Filter(filter));
    }

    // POST: api/book
    [HttpPost]
    public ActionResult<Book> Create([FromBody] Book book)
    {
        if (_repository.ExistsByTitle(book.Title))
        {
            ModelState.AddModelError(
                nameof(book.Title),
                "Книга с таким названием уже существует.");
            return ValidationProblem(ModelState);
        }

        _repository.Add(book);

        return CreatedAtAction(nameof(GetById),
            new { id = book.Id }, book);
    }

    // PUT: api/book/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Book updated)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();

        book.Title = updated.Title;
        book.Author = updated.Author;
        book.Price = updated.Price;

        _repository.Update(book);
        return NoContent();
    }

    // DELETE: api/book/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();

        _repository.Delete(book);
        return NoContent();
    }
}