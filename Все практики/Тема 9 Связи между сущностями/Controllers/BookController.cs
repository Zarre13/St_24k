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
    public ActionResult<IEnumerable<Book>> GetAll(
    [FromHeader(Name = "X-Client-Version")]
    string? clientVersion)
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{bookId:int}")]
    public ActionResult<Book> GetById( 
    [FromRoute(Name = "bookId")] int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Book>> Search(
        [FromQuery] string? genre,
        [FromQuery] decimal? minPrice)
    {
        return Ok(_repository.Search(genre, minPrice));
    }

    [HttpGet("by-name/{code}")]
    public ActionResult<Book> GetByCode(string code)
    {
       return Ok(_repository.GetByName(code));
    }

    [HttpGet("filter")]
    public ActionResult<IEnumerable<Book>> Filter(
        [FromQuery] BookFilter filter)
    {     
        return Ok(_repository.Filter(filter));
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
    