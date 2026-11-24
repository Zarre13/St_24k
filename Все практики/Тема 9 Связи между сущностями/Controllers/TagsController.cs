using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Repositories;

namespace Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _repository;

    public TagsController(ITagRepository repository)
        => _repository = repository;

    [HttpGet]
    public ActionResult<IEnumerable<Tag>> GetAll()
        => Ok(_repository.GetAll());

    [HttpPost]
    public ActionResult<Tag> Create([FromBody] Tag tag)
    {
        _repository.Add(tag);
        return Ok(tag);
    }
}