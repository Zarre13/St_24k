namespace Library.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    public int GenreId { get; set; }
    
    public Genre? Genre { get; set; } = null!;

    public decimal Price { get; set; }

    public List<Tag> Tags { get; set; } = new();
}