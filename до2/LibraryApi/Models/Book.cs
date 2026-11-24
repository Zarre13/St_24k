using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;
public class Book
{
    public int Id { get; set; }
    
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Author { get; set; } = string.Empty;

    [Range(1000, 2025)]
    public int Year { get; set; }

    [Required, StringLength(20)]
    public string Isbn { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;
}