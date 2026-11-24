using Library.Models;

namespace Library.Repositories;

public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetById(int id);
    List<Book> Search(string? author, decimal? minPrice);
    List<Book> Filter(BookFilter filter);
    bool ExistsByTitle(string? title);
    void Add(Book book);
    void Update(Book book);
    void Delete(Book book);
}
