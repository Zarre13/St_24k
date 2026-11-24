using System.ComponentModel.DataAnnotations;

namespace Automagazin.Validation;

public sealed class AllowedCarsAttribute : ValidationAttribute
{
    private static readonly string[] Allowed = ["Audi", "BMW", "Lada"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string carName && Allowed.Contains(carName))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"Название машин должно быть одним из: {string.Join(", ", Allowed)}");
    }
}
