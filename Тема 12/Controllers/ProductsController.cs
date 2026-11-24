using CatalogApiG.Repositories;
using CatalogApiG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogApiG.Models.Dto;


namespace CatalogApiG.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private const int MaxPageSize = 100; 
    public ProductsController(IProductRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
        [FromQuery] string? category, [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice, [FromQuery] string? search,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, MaxPageSize);

        IQueryable<Product> query = _repository.GetQueryable().Include(p => p.Category);
        query = _repository.Filter(query, category, minPrice, maxPrice, search);
        query = _repository.Sort(query, sortBy, sortDir);

        var (items, totalCount) = 
            await _repository.PaginateAsync(query, pageNumber, pageSize);
        var dtoItems = items.Select(p => p.ToDto()).ToList();

        return Ok(new PagedResult<ProductDto>
        {
            Items = dtoItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product =   await _repository.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        var product = dto.ToEntity();
        var created = await _repository.AddAsync(product);

        return CreatedAtAction(nameof(GetById),
            new {id = created.Id }, created.ToDto());
    }
}