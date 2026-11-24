using System.ComponentModel.DataAnnotations;

namespace Library.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = new();
}