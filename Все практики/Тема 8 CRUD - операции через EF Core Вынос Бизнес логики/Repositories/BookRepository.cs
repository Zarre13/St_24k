using Library.Data;
using Library.Models;

namespace Library.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Book> GetAll()
        => _context.Books.ToList();

    public Book? GetById(int id)
        => _context.Books.FirstOrDefault(book => book.Id == id);

    public List<Book> Search(string? author, decimal? minPrice)
    {
        var query = _context.Books.AsQueryable();

        if (author != null)
            query = query.Where(book => book.Author == author);

        if (minPrice != null)
            query = query.Where(book => book.Price >= minPrice);

        return query.ToList();
    }

    public List<Book> Filter(BookFilter filter)
    {
        var query = _context.Books.AsQueryable();

        if (filter.Author != null)
            query = query.Where(book => book.Author == filter.Author);

        if (filter.MinPrice != null)
            query = query.Where(book => book.Price >= filter.MinPrice);

        if (filter.MaxPrice != null)
            query = query.Where(book => book.Price <= filter.MaxPrice);

        return query.ToList();
    }

    public bool ExistsByTitle(string? title)
        => _context.Books.Any(book => book.Title == title);

    public void Add(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}
