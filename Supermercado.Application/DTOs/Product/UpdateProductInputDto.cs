namespace Supermercado.Application.DTOs.Product;

public record UpdateProductInputDto(string Name, string Description, Guid CategoryId);
