using System.ComponentModel.DataAnnotations;

namespace CatalogApiG.Models;

public class Product
{
    public int Id {get; set;}
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name {get; set;} = string.Empty;
    [Range(0.01, 100000)]
    public decimal Price {get; set;}
    public int CategoryId {get; set;}
    public Category? Category {get; set;}
}
