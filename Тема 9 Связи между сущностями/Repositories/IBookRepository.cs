using Library.Models;

namespace Library.Repositories;

public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetById(int bookId);
    Book? GetByName(string name);
    List<Book> Search(string? auth, decimal? minPrice);
    List<Book> Filter(BookFilter filter);
    void Add(Book book);
    void Update(Book book);
    void Delete(Book book);
    bool ExistsByName(string? title);
}