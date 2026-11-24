using CatalogApiG.Models;

namespace CatalogApiG.Repositories;

public interface ICategoryRepository
{
    
    List<Category> GetAll();
    Category? GetById(int id);
    void Add(Category category);
}