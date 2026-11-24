using Library.Models;

namespace Library.Repositories;

public interface IBookRepository
{
    IQueryable<Book> GetQueryable();
    IQueryable<Book> Filter(
        IQueryable<Book> query,
        string? genre,
        decimal? minPrice,
        decimal? maxPrice);
    IQueryable<Book> Sort(IQueryable<Book> query, string? sortBy, string? sortDir);
    PagedResult<Book> Paginate(IQueryable<Book> query, int pageNumber, int pageSize);
    Book? GetById(int bookId);
    Book? GetByName(string name);
    Book Add(Book book);
    void Update(Book book);
    void Delete(Book book);
    bool ExistsByName(string? title);
}