using System.ComponentModel.DataAnnotations;

namespace Library.Validation
{
    public class AllowedAuthorsAttribute : ValidationAttribute
    {
        private static readonly string[] Allowed = 
        { "Лермонтов", "Достоевский", "Тургенев", "Толстой", "Пушкин", "Паустовский", "Гоголь" };

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            if (value is string author && Allowed.Contains(author))
                return ValidationResult.Success;
            
            return new ValidationResult(
                $"Автор должен входить в список разрешенных авторов: {string.Join(", ", Allowed)}");
        }

    }
}