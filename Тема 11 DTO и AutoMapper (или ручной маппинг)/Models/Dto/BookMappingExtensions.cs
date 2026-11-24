using Library.Models;

namespace Library.Models.Dto;

public static class BookMappingExtensions
{
    public static BookDto ToDto(this Book book)
        => new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            GenreId = book.GenreId,
            GenreName = book.Genre?.Name,
            Price = book.Price,
            Tags = book.Tags.Select(tag => tag.Name).ToList()
        };

    public static Book ToEntity(this CreateBookDto dto)
        => new()
        {
            Title = dto.Title,
            Author = dto.Author,
            GenreId = dto.GenreId,
            Price = dto.Price
        };
}
