using CatalogApiG.Models;

namespace CatalogApiG.Repositories;

public interface IProductRepository
{   
    Task<(List<Product> Items, int TotalCount)> PaginateAsync(
        IQueryable<Product> query, int pageNumber, int pageSize);
    IQueryable<Product> Sort(
        IQueryable<Product> query, string? sortBy, string? sortDir);
    IQueryable<Product> Filter(IQueryable<Product> query,
    string? category, decimal? minPrice,
    decimal? maxPrice, string? search);
    IQueryable<Product> GetQueryable();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
}