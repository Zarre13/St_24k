using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Repositories;


namespace Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController: ControllerBase
{
    private readonly IBookRepository _repository;
    private readonly IGenreRepository _genreRepository;
    private readonly ITagRepository _tagRepository;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public BookController(
        IBookRepository repository,
        IGenreRepository genreRepository,
        ITagRepository tagRepository)
    {
        _repository = repository;
        _genreRepository = genreRepository;
        _tagRepository = tagRepository;
    }

    [HttpGet]
    public ActionResult<PagedResult<Book>> GetAll(
        [FromQuery] string? genre,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDir,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = DefaultPageSize)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var query = _repository.GetQueryable();
        query = _repository.Filter(query, genre, minPrice, maxPrice);
        query = _repository.Sort(query, sortBy, sortDir);
        var result = _repository.Paginate(query, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{bookId:int}")]
    public ActionResult<Book> GetById( 
    [FromRoute(Name = "bookId")] int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpGet("by-name/{code}")]
    public ActionResult<Book> GetByCode(string code)
    {
       return Ok(_repository.GetByName(code));
    }


    [HttpPost]
    public ActionResult<Book> Create([FromBody] Book book)
    {
        if (_repository.ExistsByName(book.Title))
        {
            ModelState.AddModelError(
                nameof(book.Title),
                "Книга с таким названием уже существует.");
            return ValidationProblem(ModelState);
        }

        if (_genreRepository.GetById(book.GenreId) == null)
        {
            ModelState.AddModelError(
                nameof(book.GenreId),
                "Жанр с таким ID не найден!");
            return ValidationProblem(ModelState);
            
        }

        _repository.Add(book);
        
        return CreatedAtAction(nameof(GetById),
            new { bookId = book.Id }, book);
    }

    [HttpPost("{bookId:int}/tags/{tagId:int}")]
    public IActionResult AddTag(int bookId, int tagId)
    {
        var book = _repository.GetById(bookId);
        if (book == null) return NotFound();

        var tag = _tagRepository.GetById(tagId);
        if (tag == null) return NotFound();

        if (book.Tags.Any(existingTag => existingTag.Id == tagId))
            return NoContent();

        book.Tags.Add(tag);
        _repository.Update(book);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Book updated)
    {
        var book  = _repository.GetById(id);
        if (book == null) return NotFound();

        book.Title = updated.Title;
        book.Author = updated.Author;
        book.Genre = updated.Genre;
        book.Price = updated.Price;

        _repository.Update(book);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {   
        var book = _repository.GetById(id);
        if (book == null) return NotFound();
        
        _repository.Delete(book);

        return NoContent();
    }

    [HttpDelete("{bookId:int}/tags/{tagId:int}")]
    public IActionResult RemoveTag(int bookId, int tagId)
    {
        var book = _repository.GetById(bookId);
        if (book == null) return NotFound();

        var tag = book.Tags.FirstOrDefault(t => t.Id == tagId);
        if (tag == null) return NotFound();

        book.Tags.Remove(tag);
        _repository.Update(book);
        return NoContent();
    }


}
    