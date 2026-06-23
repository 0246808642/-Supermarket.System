namespace Supermercado.Application.DTOs.Product;

public record RegisterProductInputDto(
    string Name,
    string Description,
    string Barcode,
    decimal Price,
    Guid CategoryId
);
