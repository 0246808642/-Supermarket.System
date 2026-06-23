namespace Supermercado.Application.DTOs.Product;

public record ProductOutputDto(
    Guid Id,
    string Name,
    string Description,
    string Barcode,
    decimal Price,
    int StockQuantity,
    bool IsActive,
    Guid CategoryId,
    DateTime ExpirationDate,
    decimal ExpirationDiscountPercentage,
    bool IsAvailable,
    decimal CurrentPrice,
    decimal DiscountedPrice
);
