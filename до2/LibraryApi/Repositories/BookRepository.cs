using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync(
    string? author = null,
    bool? available = null,
    int page = 1,
    int limit = 10)
{
    var query = _context.Books.AsQueryable();

    if (!string.IsNullOrWhiteSpace(author))
        query = query.Where(book => book.Author.Contains(author));

    if (available.HasValue)
        query = query.Where(book => book.IsAvailable == available.Value);

    return await query
        .OrderBy(book => book.Id)
        .Skip((page - 1) * limit)
        .Take(limit)
        .ToListAsync();
}
      

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books.FindAsync(id);

    public async Task<bool> IsbnExistsAsync(string isbn, int? excludingId = null)
    {
        var query = _context.Books.Where(book => book.Isbn == isbn);
        if (excludingId.HasValue)
            query = query.Where(book => book.Id != excludingId.Value);

        return await query.AnyAsync();
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null)
            return;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}