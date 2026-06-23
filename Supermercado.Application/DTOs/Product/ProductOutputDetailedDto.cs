namespace Supermercado.Application.DTOs.Product;

public record ProductOutputDetailedDto(
    Guid Id, string Name, decimal CurrentPrice, bool IsAvailable,
    string Description, string Barcode, decimal Price, int StockQuantity,
    bool IsActive, Guid CategoryId, DateTime ExpirationDate, decimal ExpirationDiscountPercentage)
    : ProductOutputDto(Id, Name, CurrentPrice, IsAvailable);
