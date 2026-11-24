using CatalogApiG.Repositories;
using CatalogApiG.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApiG.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repository;
    public CategoriesController(ICategoryRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Category>> GetAll()
        => Ok(_repository.GetAll());

    [HttpPost]
    public ActionResult<Category> Create([FromBody] Category category)
    {
        _repository.Add(category);
        return CreatedAtAction(nameof(GetAll), category);
    }
}