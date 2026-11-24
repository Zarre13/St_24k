using System.ComponentModel.DataAnnotations;

namespace Library.Validation
{
    public class AllowedGenresAttribute : ValidationAttribute
    {
        private static readonly string[] Allowed = 
        { "Роман", "Рассказ", "Поэма", "Комедия", "Трагедия", "Научно-популярный", "Новелла", "Повесть" };

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            if (value is string genre && Allowed.Contains(genre))
                return ValidationResult.Success;
            
            return new ValidationResult(
                $"Жанр должен входить в список доступных жанров: {string.Join(", ", Allowed)}");
        }

    }
}