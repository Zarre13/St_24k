using Library.Data;
using Library.Models;

namespace Library.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _context;

    public GenreRepository(AppDbContext context)
        => _context = context;

    public List<Genre> GetAll()
        => _context.Genres.ToList();

    public Genre? GetById(int genreId)
        => _context.Genres.FirstOrDefault(c => c.Id == genreId);

    public void Add(Genre genre)
    {
        _context.Genres.Add(genre);
        _context.SaveChanges();
    }
}
