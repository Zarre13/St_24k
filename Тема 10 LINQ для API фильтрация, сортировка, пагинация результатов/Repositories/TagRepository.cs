using Library.Data;
using Library.Models;

namespace Library.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    => _context = context;

    public List<Tag> GetAll()
    => _context.Tags.ToList();

    public Tag? GetById(int tagId)
    => _context.Tags.FirstOrDefault(c => c.Id == tagId);

    public void Add(Tag tag)
    {
        _context.Tags.Add(tag);
        _context.SaveChanges();
    }
}