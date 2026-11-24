namespace Library.Models;
using System.ComponentModel.DataAnnotations;
using Library.Validation;

public class Book
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 2)]
    public string? Title { get; set; }
    
    [Required]
    [AllowedAuthors]
    public string? Author { get; set; }
    [Range(0.01, 100000, ErrorMessage = "Цена должна быть от 0.01 до 100000")]
    public decimal Price { get; set; }
    
}