using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context) 
    => _context = context;

    public IQueryable<Book> GetQueryable()
        => _context.Books
            .Include(b => b.Genre)
            .Include(b => b.Tags);

    public IQueryable<Book> Filter(
        IQueryable<Book> query,
        string? genre,
        decimal? minPrice,
        decimal? maxPrice)
    {
        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(b => b.Genre != null && b.Genre.Name == genre);

        if (minPrice.HasValue)
            query = query.Where(b => b.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(b => b.Price <= maxPrice.Value);

        return query;
    }

    public IQueryable<Book> Sort(IQueryable<Book> query, string? sortBy, string? sortDir)
    {
        var descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy?.Trim().ToLowerInvariant() switch
        {
            "title" => descending
                ? query.OrderByDescending(b => b.Title).ThenBy(b => b.Id)
                : query.OrderBy(b => b.Title).ThenBy(b => b.Id),
            "author" => descending
                ? query.OrderByDescending(b => b.Author).ThenBy(b => b.Id)
                : query.OrderBy(b => b.Author).ThenBy(b => b.Id),
            "price" => descending
                ? query.OrderByDescending(b => b.Price).ThenBy(b => b.Id)
                : query.OrderBy(b => b.Price).ThenBy(b => b.Id),
            _ => query.OrderBy(b => b.Id)
        };
    }

    public PagedResult<Book> Paginate(IQueryable<Book> query, int pageNumber, int pageSize)
    {
        var totalCount = query.Count();
        var skip = (int)Math.Min((long)(pageNumber - 1) * pageSize, int.MaxValue);
        var items = query
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Book>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public Book? GetById(int bookId)
        => _context.Books
            .Include(b => b.Genre)
            .Include(b => b.Tags)
            .FirstOrDefault(b => b.Id == bookId);

    public Book? GetByName(string title)
        => _context.Books
            .Include(b => b.Genre)
            .Include(b => b.Tags)
            .FirstOrDefault(b => b.Title == title);

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

        public bool ExistsByName(string? title)
            => _context.Books.Any(p => p.Title == title);
    }