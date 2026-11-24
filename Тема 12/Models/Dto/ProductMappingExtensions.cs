namespace CatalogApiG.Models.Dto;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto (this Product product)
    => new ProductDto
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        CategoryName = product.Category?.Name ?? string.Empty
    };
    public static Product ToEntity (this CreateProductDto dto)
    => new Product
    {
        Name = dto.Name,
        Price = dto.Price,
        CategoryId = dto.CategoryId
    };
}