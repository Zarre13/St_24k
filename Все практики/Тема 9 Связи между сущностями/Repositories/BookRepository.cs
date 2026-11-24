using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context) 
    => _context = context;

    public List<Book> GetAll()
        => _context.Books
            .Include(b => b.Genre)
            .Include(b => b.Tags)
            .ToList();

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

    public List<Book> Search(string? genre, decimal? minPrice)
    {
        var query = _context.Books
            .Include(b => b.Genre)
            .Include(b => b.Tags)
            .AsQueryable();

        if (genre != null)
            query = query.Where(b => b.Genre!.Name == genre);

        if (minPrice != null)
            query = query.Where(b => b.Price >= minPrice);

        return query.ToList();
    }

    public List<Book> Filter(BookFilter filter)
    {
        var query = _context.Books
        .Include(b => b.Genre)
        .Include(b => b.Tags)
        .AsQueryable();

        if (filter.Genre != null)
            query = query.Where(b => b.Genre!.Name == filter.Genre);
        if (filter.MinPrice != null)
            query = query.Where(b => b.Price >= filter.MinPrice);
        if (filter.MaxPrice != null)
            query = query.Where(b => b.Price <= filter.MaxPrice);

        return query.ToList();
    }

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