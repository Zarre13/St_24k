using LibraryApi.Models;

namespace LibraryApi.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync(string? author = null, bool? available = null, int page = 1, int limit = 10);
    Task<Book?> GetByIdAsync(int id);
    Task<bool> IsbnExistsAsync(string isbn, int? excludingId = null);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(int id);
}