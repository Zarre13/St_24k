using System.ComponentModel.DataAnnotations;

namespace Library.Models.Dto;

public class CreateBookDto
{
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    public int GenreId { get; set; }

    [Range(0.01, 100000, ErrorMessage = "Цена должна быть от 0.01 до 100000")]
    public decimal Price { get; set; }
}
