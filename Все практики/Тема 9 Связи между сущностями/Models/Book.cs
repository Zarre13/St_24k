namespace Library.Models;
using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 2)]
    public string? Title { get; set; }
    
    [Required]
    public string? Author { get; set; }

    public int GenreId { get; set; }
    
    public Genre? Genre { get; set; } = null!;

    [Range(0.01, 100000, ErrorMessage = "Цена должна быть от 0.01 до 100000")]
    public decimal Price { get; set; }

    public List<Tag> Tags { get; set; } = new();
}