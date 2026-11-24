using System.ComponentModel.DataAnnotations;
using Automagazin.Validation;

namespace Automagazin.Models;

public class Cars_Stock
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [AllowedCars]
    public string Car_name { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Amount_of_car_in_stock { get; set; }

    [Required]
    public string Company_name { get; set; } = string.Empty;

    [Range(25000, 1000000)]
    public int Price { get; set; }

    [Required]
    public string Production_date { get; set; } = string.Empty;
}

public class Cars_Stock_Filter : IValidatableObject
{
    public string? Car_name { get; set; }
    public decimal? minPrice { get; set; }
    public decimal? maxPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
        {
            yield return new ValidationResult(
                "minPrice не может быть больше maxPrice",
                new[] { nameof(minPrice), nameof(maxPrice) });
        }
    }
}
