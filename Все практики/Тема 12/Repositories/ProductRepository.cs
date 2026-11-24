using CatalogApiG.Data;
using CatalogApiG.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApiG.Repositories;

public class ProductRepository : IProductRepository
{
    public readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public IQueryable<Product> GetQueryable() 
        => _context.Products.AsQueryable();

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await _context.Products
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == product.Id);
    }

    public IQueryable<Product> Filter(IQueryable<Product> query,
    string? category, decimal? minPrice,
    decimal? maxPrice, string? search)
    {
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category!.Name == category);
        if (minPrice.HasValue)
            query= query.Where(p => p.Price >= minPrice.Value);
        if (maxPrice.HasValue)
            query= query.Where(p => p.Price <= maxPrice.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search));

        return query; 
    }

    public IQueryable<Product> Sort(
        IQueryable<Product> query, string? sortBy, string? sortDir)
    {
        bool desc = sortDir?.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "price" => desc
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "name" => desc
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            _ => query.OrderBy(p => p.Id)  
        };
    }

    public async Task<(List<Product> Items, int TotalCount)> 
    PaginateAsync(IQueryable<Product> query,
        int pageNumber, int pageSize)
    {
        var totlaCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totlaCount);
    }
}