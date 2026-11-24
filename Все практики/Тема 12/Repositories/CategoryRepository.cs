using CatalogApiG.Data;
using CatalogApiG.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApiG.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) => _context = context;

    public List<Category> GetAll() 
        => _context.Categories.ToList();

    public Category? GetById(int id)
        => _context.Categories.FirstOrDefault(c => c.Id == id);

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
}