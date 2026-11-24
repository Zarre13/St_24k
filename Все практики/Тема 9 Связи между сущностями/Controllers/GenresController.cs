using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Repositories;

namespace Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreRepository _repository;

    public GenresController(IGenreRepository repository)
        => _repository = repository;

    [HttpGet]
    public ActionResult<IEnumerable<Genre>> GetAll()
        => Ok(_repository.GetAll());

    [HttpPost]
    public ActionResult<Genre> Create([FromBody] Genre genre)
    {
        _repository.Add(genre);
        return Ok(genre);
    }
}