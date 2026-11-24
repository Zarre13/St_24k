namespace Library.Models.Dto;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public string? GenreName { get; set; }
    public decimal Price { get; set; }
    public List<string> Tags { get; set; } = new();
}
