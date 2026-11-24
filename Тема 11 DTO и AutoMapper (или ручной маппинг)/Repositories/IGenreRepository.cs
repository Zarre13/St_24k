using Library.Models;

namespace Library.Repositories;

public interface IGenreRepository
{
    List<Genre> GetAll();
    Genre? GetById(int genreId);
    void Add(Genre genre);
}