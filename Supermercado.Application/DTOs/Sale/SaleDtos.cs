namespace Supermercado.Application.DTOs.Sale;

public record CreateSaleDto(IEnumerable<CreateSaleItemDto> Items);
public record CreateSaleItemDto(Guid ProductId, int Quantity);
public record SaleOutputDto(Guid Id, DateTime SaleDate, decimal TotalAmount, Guid CashierId, IEnumerable<SaleItemOutputDto> Items);
public record SaleItemOutputDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
