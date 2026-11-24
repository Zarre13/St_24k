using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class BookFilter : IValidatableObject
    {
        public string? Author { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext context)
        {
            if (MinPrice != null && MaxPrice != null && MinPrice > MaxPrice)
            {
                yield return new ValidationResult(
                    "MinPrice не может быть больше MaxPrice",
                    new[] { nameof(MinPrice), nameof(MaxPrice) });
            }
        }
    }
}