using Library.Models;

namespace Library.Repositories;

public interface ITagRepository
{
    List<Tag> GetAll();
    Tag? GetById(int tagId);
    void Add(Tag tag);
}