namespace Supermercado.Application.DTOs.Product;

public record ProductOutputAuditDto(
    Guid Id, string Name, decimal CurrentPrice, bool IsAvailable,
    string Description, string Barcode, decimal Price, int StockQuantity,
    bool IsActive, Guid CategoryId, DateTime ExpirationDate, decimal ExpirationDiscountPercentage,
    Guid CreatedByUserId, DateTime CreatedAt, Guid? RemovedByUserId, DateTime? RemovedAt, bool IsRemoved)
    : ProductOutputDetailedDto(Id, Name, CurrentPrice, IsAvailable, Description, Barcode, Price,
        StockQuantity, IsActive, CategoryId, ExpirationDate, ExpirationDiscountPercentage);
